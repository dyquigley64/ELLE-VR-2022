using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HellesKitchenManager : MonoBehaviour
{
    //public Transform[] ingredients;

    public GameObject finishedPlate;
    public GameObject pot;
    public GameObject chefCounter;
    public GameObject chefAudio;
    public GameObject bell;
    public GameMenu menu;
    public GameObject LocomotionSystem;
    public GameObject recipeCard;
    public GameObject XRRig;
    public GameObject ingredientBoard;
    public GameObject ingredientBoardText;
    public GameObject grabTutorialBlock;
    public GameObject tutorialTable;
    public GameObject correctTutorialBlock;
    public GameObject incorrectTutorialBlock;
    public GameObject tutorialGolfBall;
    public GameObject narration;
    private GameObject ingredientBoardImage;
    private GameObject[] ingredientBlocks;
    private GameObject[] shuffledIngredientBlocks;

    public Material scoreBackground;


    private AudioSource sfx;

    // Tutorial audio
    public AudioClip tutorialChefAudio;
    public AudioClip tutorialFinishedPlateAudio;

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;
    public AudioClip clip5;
    public AudioClip clip6;
    public AudioClip clip7;
    public AudioClip clip8;
    public AudioClip clip9;
    public AudioClip clip10;
    public AudioClip clip11;
    public AudioClip clip12;
    public AudioClip clip13;
    public AudioClip clip14;
    public AudioClip clip15;
    public AudioClip clip16;
    public AudioClip clip17;
    public AudioClip clip18;
    public AudioClip clip19;
    public AudioClip clip20;
    public AudioClip clip21;

    public Texture tutorialFinishedPlateTexture;

    public Material defaultMaterial;

    private Term[] answerArray;

    public List<Term> termsBag;
    public List<GameObject> deactivedIngredientBlocks;
    public List<GameObject> populatedIngredientBlocks;
    public List<string> answerList = new List<string>();
    public static List<LongQuestion> questionList;
    private List<string> foodList = new List<string>();
    private List<Term> termList;
    private List<Term> dummyTermList;
    private List<Vector3> ingredientPositions;
    private List<Quaternion> ingredientRotations;

    public Quaternion initialPlayerRotation;
    private Quaternion initialRecipeCardRotation;
    private Quaternion initialFinishedPlateRotation;

    public GameMode currentGameMode = GameMode.Quiz;

    public bool skipTutorial;
    public bool canCheckAnswer;
    public bool movementEnabled;
    public bool testing = false;
    private bool startGame = false;
    private bool teleportSelected = false;
    private bool tutorialDone = false;
    private bool timerStart = false;
    private bool incrementRoundCounter = false;

    public static int currentRound;
    public int sessionID;

    public int score = 0;
    private int realScore = 0;

    public int attempts = 0;

    private float timer;

    private bool dontLeaveTooEarlyFlag;
    

    // Start is called before the first frame update
    void Awake()
    {
        sfx = narration.GetComponent<AudioSource>();
        sfx.playOnAwake = false;

        // Hide all tutorial objects
        grabTutorialBlock.SetActive(false);
        tutorialTable.SetActive(false);
        correctTutorialBlock.SetActive(false);
        incorrectTutorialBlock.SetActive(false);
        tutorialGolfBall.SetActive(false);

        deactivedIngredientBlocks = new List<GameObject>();
        populatedIngredientBlocks = new List<GameObject>();

        ingredientPositions = new List<Vector3>();
        ingredientRotations = new List<Quaternion>();

        finishedPlate.SetActive(false);
        finishedPlate.transform.position = new Vector3(3.27511597f, 1.10839939f, 5.59656668f);

        if (LocomotionSystem.GetComponent<TeleportationManager>().teleportMovement)
            teleportSelected = true;


        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        
        menu.onStartGame = StartGame;
        menu.onTutorialGame = StartGame;
        termsBag = new List<Term>();

        initialPlayerRotation = XRRig.transform.rotation;
        initialRecipeCardRotation = recipeCard.transform.rotation;
        initialFinishedPlateRotation = finishedPlate.transform.rotation;

        // ingredientBlocks is an array that holds every ingredient block in the scene (as long as it has the tag)
        ingredientBlocks = GameObject.FindGameObjectsWithTag("Ingredient Block");

        // Going through and storing the positions and rotations of every ingredient block in order to reset them between rounds
        foreach (var block in ingredientBlocks)
        {
            ingredientPositions.Add(block.transform.position);
            ingredientRotations.Add(block.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        menu.goodToLeave = !startGame && !dontLeaveTooEarlyFlag;

        if (timerStart)
		{
            timer -= Time.deltaTime;
		}

        if (startGame)
        {
            startGame = false;
            StartCoroutine(LaunchModule());
        }

        // If the player drops the finished plate onto the chefs' counter, reset the round
        if (chefCounter.GetComponent<ChefCounter>().roundFinished && ((currentGameMode != GameMode.Tutorial) || (currentGameMode == GameMode.Tutorial && tutorialDone)))
        {
            chefCounter.GetComponent<ChefCounter>().roundFinished = false;

            ResetRoundWrapper(false);
        }
    }

    private IEnumerator LaunchModule()
	{
        startGame = false;
        currentGameMode = menu.currentGameMode;

        if (!tutorialDone && currentGameMode == GameMode.Tutorial)
        {
            yield return Tutorial();
            tutorialDone = true;
            StartCoroutine(GetItStarted());
        }
        else
		{
            incrementRoundCounter = true;
            StartCoroutine(GetItStarted());
        }
            
    }

    private IEnumerator Tutorial()
	{
        float audioClipLength;

        /* INTIAL SETUP */
        // --------------------------------------------------------------------
        Vector3 grabBlockInitialPosition = grabTutorialBlock.transform.position;
        Quaternion grabBlockInitialRotation = grabTutorialBlock.transform.rotation;

        // Despawn recipe card
        recipeCard.SetActive(false);

        // Deactivate all regular blocks in the scene
        foreach (var block in ingredientBlocks)
        {
            block.SetActive(false);
        }

        // Reset and freeze player position and rotation
        XRRig.transform.position = new Vector3(7.81f, 0, -1.399f);
        XRRig.transform.rotation = initialPlayerRotation;
        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        PlaySoundEffect(clip1);
        yield return new WaitWhile(() => sfx.isPlaying);

        yield return StartCoroutine(AssignChefClip());

        yield return new WaitForSeconds(1f);
        // --------------------------------------------------------------------



        /* CHEF AUDIO & BELL*/
        // --------------------------------------------------------------------
        PlaySoundEffect(clip2);
        yield return new WaitWhile(() => sfx.isPlaying);

        audioClipLength = PlaySoundEffect(clip3);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        bell.GetComponent<Bell>().tutorialBell = true;

        timer = 5f;
        timerStart = true;

        while (bell.GetComponent<Bell>().tutorialBell)
        {
            // Pause for 1/4 second at a time until the player hits the bell
            yield return new WaitForSeconds(0.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip3);
                timer = 5f + audioClipLength;
            }

        }

        timerStart = false;
        sfx.Stop();

        yield return new WaitForSeconds(2f);

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        // --------------------------------------------------------------------



        /* PROXIMITY GRABBING */
        // --------------------------------------------------------------------
        // Spawn the table used in the grabbing tutorial
        tutorialTable.SetActive(true);

        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        // Move player in front of racks
        XRRig.transform.position = new Vector3(6.88100004f, 0, -6.26599979f);
        XRRig.transform.rotation = initialPlayerRotation;

        // Spawn the block that will be grabbed
        grabTutorialBlock.SetActive(true);

        PlaySoundEffect(clip4);
        yield return new WaitWhile(() => sfx.isPlaying);

        audioClipLength = PlaySoundEffect(clip5);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        timer = 5f;
        timerStart = true; ;

        while (!grabTutorialBlock.GetComponent<GrabTutorialBlock>().grabbed)
        {
            // Pause for 1/4 second at a time until the player grabs the block 
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip5);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        yield return new WaitForSeconds(1f);

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        // --------------------------------------------------------------------



        /* RAYCAST GRABBING */
        // --------------------------------------------------------------------
        // Reset the block for the raycast tutorial        
        grabTutorialBlock.GetComponent<GrabTutorialBlock>().grabbed = false;
        grabTutorialBlock.transform.position = grabBlockInitialPosition;
        grabTutorialBlock.transform.rotation = grabBlockInitialRotation;


        // Move player in front of racks
        XRRig.transform.position = new Vector3(6.88100004f, 0, -6.26599979f);
        XRRig.transform.rotation = initialPlayerRotation;

        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();
        PlaySoundEffect(clip6);
        yield return new WaitWhile(() => sfx.isPlaying);

        audioClipLength = PlaySoundEffect(clip7);
        yield return new WaitWhile(() => sfx.isPlaying);


        // Voice over explaining grabbing with raycast
        timer = 5f;
        timerStart = true;

        while (!grabTutorialBlock.GetComponent<GrabTutorialBlock>().grabbed)
        {
            // Pause for 1/4 second at a time until the player grabs the block 
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip7);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        yield return new WaitForSeconds(1f);
        // --------------------------------------------------------------------



        /* DROPPING BLOCKS IN POT */
        // --------------------------------------------------------------------
        // Set pot to tutorial mode
        pot.GetComponent<CheckFoodAnswer>().isTutorial = true;
        pot.GetComponent<CheckFoodAnswer>().isTutorialSmiley = true;
        pot.GetComponent<CheckFoodAnswer>().isTutorialFrowny = true;

        // Spawn blocks
        correctTutorialBlock.SetActive(true);
        incorrectTutorialBlock.SetActive(true);

        // Move player in front of the pot
        XRRig.transform.position = new Vector3(3.27600002f, 0, 4.83699989f);
        XRRig.transform.rotation = new Quaternion(0, 0, 0, 1.0f);
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        PlaySoundEffect(clip8);
        yield return new WaitWhile(() => sfx.isPlaying);

        audioClipLength = PlaySoundEffect(clip9);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        timer = 5f;
        timerStart = true;

        while (pot.GetComponent<CheckFoodAnswer>().isTutorialSmiley)
        {
            // Pause for 1/4 second at a time until the player drops the correct block
            // in the pot
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip9);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        yield return new WaitForSeconds(1f);

        PlaySoundEffect(clip10);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        audioClipLength = PlaySoundEffect(clip11);
        yield return new WaitWhile(() => sfx.isPlaying);

        timer = 5f;
        timerStart = true;

        pot.GetComponent<CheckFoodAnswer>().isTutorial = true;

        while (pot.GetComponent<CheckFoodAnswer>().isTutorialFrowny)
        {
            // Pause for 1/4 second at a time until the player drops the correct block
            // in the pot
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip11);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        yield return new WaitForSeconds(2f);

        // Voice over will explain that the pot will play a negative sound and
        // will not accept the block when its droped if it's incorrect
        // --------------------------------------------------------------------



        /* RECIPE CARD */
        // --------------------------------------------------------------------
        recipeCard.SetActive(true);
        recipeCard.GetComponent<RecipeCard>().isTutorial = true;

        recipeCard.transform.position = new Vector3(2.77620006f, 0.99879998f, 5.48169994f);
        recipeCard.transform.rotation = new Quaternion(0.5f, 0.5f, 0.5f, 0.5f);

        PlaySoundEffect(clip12);
        yield return new WaitWhile(() => sfx.isPlaying);

        audioClipLength = PlaySoundEffect(clip13);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        timer = 5f;
        timerStart = true;

        while (!recipeCard.GetComponent<RecipeCard>().placedOnBlackboard)
        {
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip13);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        audioClipLength = PlaySoundEffect(clip14);
        yield return new WaitWhile(() => sfx.isPlaying);

        timer = 5f;
        timerStart = true;

        while (!recipeCard.GetComponent<RecipeCard>().retrievedFromBlackboard)
        {
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip14);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        yield return new WaitForSeconds(1f);
        // --------------------------------------------------------------------



        /* FINISHED DISH */
        // --------------------------------------------------------------------        
        pot.GetComponent<CheckFoodAnswer>().isTutorial = true;

        PlaySoundEffect(clip15);
        yield return new WaitWhile(() => sfx.isPlaying);

        PlaySoundEffect(clip16);
        yield return new WaitWhile(() => sfx.isPlaying);

        tutorialGolfBall.SetActive(true);

        audioClipLength = PlaySoundEffect(clip17);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        timer = 5f;
        timerStart = true;

        while (pot.GetComponent<CheckFoodAnswer>().isTutorial)
        {
            // Pause for 1/4 second at a time until the player drops the correct block
            // in the pot
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip17);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        yield return new WaitForSeconds(0.5f);

        // Set up finished plate block with tutorial information
        finishedPlate.transform.name = "Golf";
        finishedPlate.GetComponent<AudioSource>().clip = tutorialFinishedPlateAudio;
        Renderer m_material = finishedPlate.transform.GetChild(0).GetComponent<Renderer>();
        m_material.material.SetTexture("_BaseMap", tutorialFinishedPlateTexture);

        // Voice over prompting player to drop finished block at the chef counter

        // Waiting for the player to drop the finished dish at the chef counter
        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        timer = 5f;
        timerStart = true;

        chefCounter.GetComponent<ChefCounter>().roundFinished = false;

        while (!chefCounter.GetComponent<ChefCounter>().roundFinished)
        {
            // Pause for 1/4 second at a time until the player drops the correct block
            // in the pot
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip17);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().DisableRaycastGrab();

        currentRound = 0;
        yield return new WaitForSeconds(1f);
        // --------------------------------------------------------------------



        /* HINTS */
        // --------------------------------------------------------------------
        // Reset the block for the hint tutorial
        grabTutorialBlock.SetActive(true);
        grabTutorialBlock.GetComponent<GrabTutorialBlock>().grabbed = false;
        grabTutorialBlock.transform.position = grabBlockInitialPosition;
        grabTutorialBlock.transform.rotation = grabBlockInitialRotation;


        // Move player in front of racks, further away
        XRRig.transform.position = new Vector3(6.88100004f, 0, -6.26599979f);
        XRRig.transform.rotation = initialPlayerRotation;

        yield return new WaitForSeconds(2f);
        PlaySoundEffect(clip18);
        yield return new WaitWhile(() => sfx.isPlaying);

        PlaySoundEffect(clip19);
        yield return new WaitWhile(() => sfx.isPlaying);

        grabTutorialBlock.transform.GetChild(0).GetComponent<Hint>().isTutorial = true;
        audioClipLength = PlaySoundEffect(clip20);
        yield return new WaitWhile(() => sfx.isPlaying);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        LocomotionSystem.GetComponent<TeleportationManager>().EnableRaycastGrab();

        timer = 5f;
        timerStart = false;

        while (grabTutorialBlock.transform.GetChild(0).GetComponent<Hint>().isTutorial)
        {
            // Pause for 1/4 second at a time until the player grabs the block 
            yield return new WaitForSeconds(.25f);

            if (timer <= 0f)
            {
                PlaySoundEffect(clip20);
                timer = 5f + audioClipLength;
            }
        }

        timerStart = false;
        sfx.Stop();

        yield return new WaitForSeconds(1f);
        // --------------------------------------------------------------------



        /* STARTING GAME */
        // --------------------------------------------------------------------
        PlaySoundEffect(clip21);
        yield return new WaitWhile(() => sfx.isPlaying);


        // Reset player position to begin game
        XRRig.transform.position = new Vector3(7.81f, 0, -1.399f);
        XRRig.transform.rotation = initialPlayerRotation;        

        // Reactivate all regular blocks in the scene
        foreach (var block in ingredientBlocks)
        {
            block.SetActive(true);
        }

        // Reset pot back to normal
        CheckFoodAnswer.isTriggered = false;
        pot.GetComponent<CheckFoodAnswer>().isTutorial = false;
        pot.GetComponent<CheckFoodAnswer>().isSpawned = false;

        // Reset finished plate position
        finishedPlate.transform.position = new Vector3(3.27511597f, 1.10839939f, 5.59656668f);
        finishedPlate.transform.rotation = initialFinishedPlateRotation;
        finishedPlate.SetActive(false);

        // Despawn the table used in the grabbing tutorial
        tutorialTable.SetActive(false);

        // Despawn the tutorial blocks
        grabTutorialBlock.SetActive(false);
        incorrectTutorialBlock.SetActive(false);
        correctTutorialBlock.SetActive(false);
        tutorialGolfBall.SetActive(false);

        // Reset recipe card position
        recipeCard.transform.position = new Vector3(8.6392f, 0.904f, -1.405f);
        recipeCard.transform.rotation = initialRecipeCardRotation;
        recipeCard.GetComponent<RecipeCard>().isTutorial = false;

        currentRound = 0;
        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        // --------------------------------------------------------------------
    }


    private IEnumerator GetItStarted()
    {
        currentRound = 0;
        termList = menu.termList;

        questionList = ELLEAPI.GetCustomQuestionsFromModule(menu.currentModule.moduleID);

        answerArray = questionList[currentRound].answers;
        
        for(int i = 0; i < answerArray.Length; ++i)
        {
            answerList.Add(answerArray[i].front);
        }


        sessionID = ELLEAPI.StartSession(menu.currentModule.moduleID, currentGameMode == GameMode.Endless);
        if (currentGameMode == GameMode.Endless)
        {
            for (int i = termList.Count - 1; i >= 0; i--)
            {
                if (menu.termEnabled[i] == false)
                    termList.RemoveAt(i);
            }
        }
        yield return new WaitForSeconds(1f);
        recipeCard.GetComponent<RecipeCard>().ResetCard();
        FillTermBag();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AddTermDataToGameObjects());
        canCheckAnswer = true;
        AddQuestionDataToGameObjects();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(AssignChefClip());
        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
    }

    public void StartGame()
    {
        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        // Will get replaced with API call done in TP Manager
        if (teleportSelected)
            LocomotionSystem.GetComponent<TeleportationManager>().teleportMovement = true;
        else
            LocomotionSystem.GetComponent<UnityEngine.XR.Interaction.Toolkit.CustomContinuousManager>().enabled = true;

        startGame = true;

        // Sets the position of the player to be right in front of the chefs' counter
        XRRig.transform.position = new Vector3(7.81f, 0, -1.399f);
    }

    public void ResetRoundWrapper(bool wait)
	{
        StartCoroutine(ResetRound(wait));
	}

    private IEnumerator ResetRound(bool wait)
	{
        // If the method calling this one asks this one to wait, wait for 5 seconds before resetting the round
        if (wait)
            yield return new WaitForSeconds(5f);

        // Increment the current round and check to see if the previous round was the last round.
        // Don't increment the round if the player just finished the tutorial.
        if (incrementRoundCounter)
            currentRound++;
        else
            incrementRoundCounter = true;


        // Reset player position and rotation
        XRRig.transform.position = new Vector3(7.81f, 0, -1.399f);
        XRRig.transform.rotation = initialPlayerRotation;
        LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();

        // Reset finished plate position
        finishedPlate.transform.position = new Vector3(3.27511597f, 1.10839939f, 5.59656668f);
        finishedPlate.transform.rotation = initialFinishedPlateRotation;

        // Clear board
        ingredientBoardText.GetComponent<TMP_Text>().text = "";
        Renderer b_material = ingredientBoard.transform.GetChild(0).GetComponent<Renderer>();
        b_material.material.SetTexture("_BaseMap", defaultMaterial.mainTexture);

        

        //FOR DEBUGGING END MENU
        //currentRound = questionList.Count;
        if (currentRound >= questionList.Count)
		{
            XRRig.transform.position = new Vector3(7.81f, 0, 1.377f);
            XRRig.transform.rotation = initialPlayerRotation;
            LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();

            StartCoroutine(FinishGame());
            
            yield break;
		}

        // Clear and + refill the answerArray & answerList
        answerArray = questionList[currentRound].answers;
        answerList.Clear();
        for (int i = 0; i < answerArray.Length; ++i)
        {
            answerList.Add(answerArray[i].front);
        }

        // Reset ingredient block positions and rotations
        for (int i = 0; i < ingredientBlocks.Length; i++)
		{
            ingredientBlocks[i].transform.position = ingredientPositions[i];
            ingredientBlocks[i].transform.rotation = ingredientRotations[i];
        }

        yield return new WaitForSeconds(1f);
        // "Respawning" all the blocks that was deactivated in the previous round
        foreach (GameObject block in deactivedIngredientBlocks)
        {
            block.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        deactivedIngredientBlocks.Clear();

        // Randomize the term list and update the ingredient blocks
        FillTermBag();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AddTermDataToGameObjects());
        canCheckAnswer = true;
        AddQuestionDataToGameObjects();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AssignChefClip());        

        // Reset recipe card position and rotation and update its content
        recipeCard.GetComponent<RecipeCard>().ResetCard();
        recipeCard.transform.position = new Vector3(8.6392f, 0.904f, -1.405f);
        recipeCard.transform.rotation = initialRecipeCardRotation;        

        pot.SetActive(true);
        CheckFoodAnswer.isTriggered = false;
        pot.GetComponent<CheckFoodAnswer>().isSpawned = false;
        CheckFoodAnswer.resetHintTimer = true;
        finishedPlate.SetActive(false);

        LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
    }

    private void FillTermBag()
    {
        termsBag.Clear();
        List<int> indicies = new List<int>();
        int currentIndex, i;

        for (i = 0; i < termList.Count; i++)
            indicies.Add(i);

        while (indicies.Count > 0)
        {
            currentIndex = Random.Range(0, indicies.Count);
            termsBag.Add(termList[indicies[currentIndex]]);
            indicies.RemoveAt(currentIndex);
        }
    }

    // Takes in no parameters. Creates a shuffled duplicate of the ingredientBLocks references and iterates through it, first assigning all available answers to the list of blocks and then assigning the remaining terms. 
    // After all of the terms have been assigned, it adds all remaining blocks to the deactivatedBlocks list and sets them all as inactive in the scene.
    private IEnumerator AddTermDataToGameObjects()
    {
        if (termsBag.Count == 0 || answerArray.Length == 0 || ingredientBlocks == null) yield break;


        if (currentRound >= 1)
        {
            for(int l = 0; l < ingredientBlocks.Length; ++l)
            {
                ingredientBlocks[l].name = "Ingredient";     
            }
        }

        // Apply correct term data to game objects
        for (int j = 0; j < termsBag.Count; ++j)
        {
            if (answerList.Contains(termsBag[j].front))
            {
                int randomIngredientIndex = Random.Range(0, ingredientBlocks.Length);

                // if the ingredient block does not contain this name then it already has a term applied
                while (!ingredientBlocks[randomIngredientIndex].name.Contains("Ingredient"))
                    randomIngredientIndex = Random.Range(0, ingredientBlocks.Length);

                // Rename gameobject to the term
                ingredientBlocks[randomIngredientIndex].name = termsBag[j].front;

                // Apply the audio clip to the game object
                ingredientBlocks[randomIngredientIndex].gameObject.GetComponent<AudioSource>().clip = termsBag[j].audio;

                // Apply image to ingredient block (Texture)
                Renderer m_material = ingredientBlocks[randomIngredientIndex].transform.GetChild(0).GetComponent<Renderer>();
                m_material.material.SetTexture("_BaseMap", termsBag[j].image);
            }
        }

        // apply the rest of the terms (not correct) to game objects
        for (int i = 0; i < termsBag.Count; i++)
        {
            if (i >= termsBag.Count) break;

            if (answerList.Contains(termsBag[i].front)) continue;

            int ingredientIndex = Random.Range(0, ingredientBlocks.Length);

            // if the ingredient block does not contain this name then it already has a term applied
            while (!ingredientBlocks[ingredientIndex].name.Contains("Ingredient"))
                ingredientIndex = Random.Range(0, ingredientBlocks.Length);

            // rename the gameobject to term 
            ingredientBlocks[ingredientIndex].name = termsBag[i].front;

            // Apply the audio clip to the game object
            ingredientBlocks[ingredientIndex].gameObject.GetComponent<AudioSource>().clip = termsBag[i].audio;

            // Apply image to ingredient block (Texture)
            Renderer m_material = ingredientBlocks[ingredientIndex].transform.GetChild(0).GetComponent<Renderer>();
            m_material.material.SetTexture("_BaseMap", termsBag[i].image);//change the texture to the image
        }
        yield return new WaitForSeconds(1f);
        for(int k = 0; k < ingredientBlocks.Length; ++k)
        {
            if (ingredientBlocks[k].name.Contains("Ingredient"))
            {
                deactivedIngredientBlocks.Add(ingredientBlocks[k]);
                ingredientBlocks[k].SetActive(false);
            }
        }
    }

    private IEnumerator AssignChefClip()
    {
        if (!tutorialDone && currentGameMode == GameMode.Tutorial)
		{
            chefAudio.GetComponent<ChefAudio>().playAgain = true;
            chefAudio.GetComponent<ChefAudio>().questionClip.clip = tutorialChefAudio;
        }
        else
		{
            if (questionList[currentRound].audio == null)
            {
                Debug.Log("Can't assign chef audio. quesitonList[currentRound] is null");
                yield break;
            }

            chefAudio.GetComponent<ChefAudio>().playAgain = true;
            chefAudio.GetComponent<ChefAudio>().questionClip.clip = questionList[currentRound].audio;
        }
    }

    private void AddQuestionDataToGameObjects()
    {
        if (questionList.Count == 0) return;

        finishedPlate.transform.name = questionList[currentRound].questionText;
        if(questionList[currentRound].audio != null)
            finishedPlate.GetComponent<AudioSource>().clip = questionList[currentRound].audio;

        if (questionList[currentRound].image != null)
        {
            Renderer m_material = finishedPlate.transform.GetChild(0).GetComponent<Renderer>();
            m_material.material.SetTexture("_BaseMap", questionList[currentRound].image);//change the texture to the image

        }

    }

    private float PlaySoundEffect(AudioClip audioClip)
    {
        if (!sfx.isPlaying)
            sfx.PlayOneShot(audioClip);

        return audioClip.length;
    }


    private IEnumerator FinishGame()
    {
        menu.background.GetComponent<Renderer>().material = scoreBackground;
        menu.EndGame(score, attempts);
        ELLEAPI.EndSession(sessionID, score);

        yield return new WaitForSeconds(2);
    }
    
}
