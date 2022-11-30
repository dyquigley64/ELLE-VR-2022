using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class CheckFoodAnswer : MonoBehaviour
{
    public HellesKitchenManager hellesKitchenManager;
    public FoodManager foodManager;
    public ParticleSystem poof;

    public AudioClip correctSfx;
    public AudioClip incorrectSfx;
    public AudioClip poofSfx;
    private AudioSource sfx;

    public GameObject poofAudio;
    public GameObject finishedPlate;
    public GameObject pot;
    public GameObject ingredientBoard;
    public GameObject ingredientBoardText;
    private GameObject ingredientBoardImage;

    public Material defaultMaterial;
    private Renderer b_material;

    public List<string> correctTerms;

    public static bool resetHintTimer;
    public bool isTutorial = false;
    public bool isTutorialSmiley = false;
    public bool isTutorialFrowny = false;
    public bool isSpawned = false;
    private bool isOnStove = true;
    public bool isPlayingPoof = false;
    public static bool isTriggered = false;
    private int numOfCorrectFood = 4;


    private bool termIdFound = false;

    //private Term currentTerm = Term temp;

    private void OnEnable()
    {
        isSpawned = false;
        isPlayingPoof = false;
}


    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        sfx.playOnAwake = false;
        poof.playOnAwake = false;
        resetHintTimer = true;

        ingredientBoardText.GetComponent<TMP_Text>().text = "";
        
        if (hellesKitchenManager.testing)
        {
            numOfCorrectFood = 4;

            correctTerms = new List<string>();
            correctTerms.Add("Golf Ball");
            correctTerms.Add("Golf Cart");
            correctTerms.Add("Golf Club");
            correctTerms.Add("Golf Tee");
        }
        /*
        else
        {
            numOfCorrectFood = hellesKitchenManager.answerList.Count;
            correctTerms = hellesKitchenManager.answerList;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (hellesKitchenManager.canCheckAnswer)
        {   
            numOfCorrectFood = hellesKitchenManager.answerList.Count;
            correctTerms = hellesKitchenManager.answerList;          
        }
    }

    // Checks for collision of food into the pot/pan or checks if the pot/pan is on a stove top
    private void OnTriggerEnter(Collider collision)
    {
        if (isTutorial)
		{
            if (collision.gameObject.name.Equals("Smiley Face") || collision.gameObject.name.Equals("Frowny Face"))
            {
                if (collision.gameObject.name.Equals("Smiley Face"))
				{
                    if (!isOnStove) return;

                    PlaySoundEffect(correctSfx);                    

                    UpdateBoard(collision.gameObject);

                    float audioClipLength = PlayTermAudio(collision.gameObject);

                    StartCoroutine(DespawnTutorialBlock(audioClipLength, collision.gameObject, true));

                    isTutorialSmiley = false;
                }
                else
				{
                    PlaySoundEffect(incorrectSfx);

                    float audioClipLength = PlayTermAudio(collision.gameObject);

                    if (!isTutorialSmiley)
					{
                        StartCoroutine(DespawnTutorialBlock(audioClipLength, collision.gameObject, false));

                        isTutorialFrowny = false;
                    }                        
                }
            }
            if (collision.gameObject.name.Equals("Golf Ball"))
			{
                if (!isOnStove) return;

                PlaySoundEffect(correctSfx);

                UpdateBoard(collision.gameObject);

                float audioClipLength = PlayTermAudio(collision.gameObject);

                StartCoroutine(DespawnTutorialBlock(audioClipLength, collision.gameObject, true));

                SpawnFinishedPlate();

                isTutorial = false;
            }
        }
        else
		{
            if (collision.gameObject.CompareTag("Ingredient Block") && !isTriggered)
            {
                isTriggered = true;
                if (!isOnStove) return;

                // Check answer then play effects
                if (IsCorrectTerm(collision.gameObject.name) && !EndOfCurrentRound())
                {               
                    // player got an answer correct score increases
                    hellesKitchenManager.score++;


                    // player attempted an answer, will increase whether correct or not
                    hellesKitchenManager.attempts++;

                    Term currentTerm = getTerm(collision.gameObject.name);

                    if(termIdFound)
                    {
                        ELLEAPI.LogAnswer(hellesKitchenManager.sessionID, currentTerm, true, hellesKitchenManager.currentGameMode == GameMode.Endless);
                        termIdFound = false;
                    }


                    PlaySoundEffect(correctSfx);

                    UpdateBoard(collision.gameObject);

                    float audioClipLength = PlayTermAudio(collision.gameObject);

                    StartCoroutine(DespawnIngredient(audioClipLength, collision.gameObject));

                    correctTerms.Remove(collision.gameObject.name);

                    

                    if (EndOfCurrentRound())
                    {

                        SpawnFinishedPlate();

                        
                    }
                    else
                        resetHintTimer = true;
                }
                else if (!EndOfCurrentRound())
                {
                    Term currentTerm = getTerm(collision.gameObject.name);

                    // player attempted an answer, will increase whether correct or not
                    hellesKitchenManager.attempts++;


                    if(termIdFound)
                    {
                        ELLEAPI.LogAnswer(hellesKitchenManager.sessionID, currentTerm, false, hellesKitchenManager.currentGameMode == GameMode.Endless);
                        termIdFound = false;
                    }

                    PlaySoundEffect(incorrectSfx);
                    
                    float audioClipLength = PlayTermAudio(collision.gameObject);

                    StartCoroutine(DespawnIngredient(audioClipLength, collision.gameObject));
                }
                
            }
            // If the pot/pan is on the stove top
            else if (collision.gameObject.CompareTag("StoveTop"))
            {
                isOnStove = true;
            }
        }
    }


    // If the pot/pan leaves the stove top
    private void OnTriggerExit(Collider collision)
    {
        //isTriggered = false;
        if (collision.gameObject.CompareTag("StoveTop"))
        {
            isOnStove = false;
        }
    }

    // Getting going through list in term bag and comparing till I find the name that matches and returning that id
    private Term getTerm(string name)
    {
        Term temp = null; 
        
        foreach (var term in hellesKitchenManager.termsBag)
        {
            if(name == term.front)
            {
                termIdFound = true;
                temp = term;
                return temp;
            }
        }

        return temp;
    }

    private void UpdateBoard(GameObject correctIngredient)
    {
        ingredientBoardText.GetComponent<TMP_Text>().text = correctIngredient.name;

        b_material = ingredientBoard.transform.GetChild(0).GetComponent<Renderer>();
        b_material.material.SetTexture("_BaseMap", correctIngredient.transform.GetChild(0).GetComponent<Renderer>().material.GetTexture("_BaseMap"));
    }

    private void SpawnFinishedPlate()
    {
        StartCoroutine(FinishedPlateTimer());
    }

    // returns true if the name passed in is found in the correctTerms List otherwise false
    private bool IsCorrectTerm(string name)
    {
        char[] charToTrim = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(', ')', ' ' };
        return correctTerms.Contains(name.Replace("Clone", ")").Trim(charToTrim));
    }

    // play the audio clip passed in
    private void PlaySoundEffect(AudioClip audioClip)
    {
        if (!sfx.isPlaying)
            sfx.PlayOneShot(audioClip);
    }

    // Plays the audioclip attached to the game object, returns the length of the audio clip
    private float PlayTermAudio(GameObject gameObject)
    {
        AudioSource termAudio = gameObject.GetComponent<AudioSource>();

        if (termAudio.clip == null) 
            return 0;

        termAudio.loop = false;
        if (!termAudio.isPlaying)
            termAudio.Play();

        return termAudio.clip.length;
    }

    // Destroys the ingredient block after a certain time
    private IEnumerator DespawnIngredient(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay);

        ingredientBoardText.GetComponent<TMP_Text>().text = "";
        b_material.material.SetTexture("_BaseMap", defaultMaterial.mainTexture);

        hellesKitchenManager.deactivedIngredientBlocks.Add(gameObject);
        isTriggered = false;
        gameObject.SetActive(false);
    }

    // Destroys the tutorial block after a certain time
    private IEnumerator DespawnTutorialBlock(float delay, GameObject gameObject, bool correct)
    {
        yield return new WaitForSeconds(delay);

        if (correct)
		{
            ingredientBoardText.GetComponent<TMP_Text>().text = "";
            b_material.material.SetTexture("_BaseMap", defaultMaterial.mainTexture);
        }

        gameObject.SetActive(false);
    }


    private IEnumerator FinishedPlateTimer()
    {
        yield return new WaitForSeconds(2f);

        if (!isSpawned) // Checks if the finishedplate has spawned yet
        {            
            isSpawned = true;

            finishedPlate.SetActive(true);
            // Play smoke effect
            if (!isPlayingPoof)
            {
                isPlayingPoof = true;
                poofAudio.GetComponent<AudioSource>().PlayOneShot(poofSfx);
                ParticleSystem tempPoof = Instantiate(poof);
                tempPoof.Play();

            }
            // Show the finisheplate image/term on the board then clear it when the round resets
            UpdateBoard(finishedPlate.gameObject);

            PlayTermAudio(finishedPlate.gameObject);
            pot.SetActive(false);
        }
    }

	private bool EndOfCurrentRound()
    {
        if (correctTerms.Count == 0)
            return true;
        else
            return false;
    }
}
