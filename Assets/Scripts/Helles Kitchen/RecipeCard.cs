using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeCard : MonoBehaviour
{
    public GameMenu menu;
    public static int currentQuestionIndex = 0;

    private static LongQuestion currentQuestion;

    public GameObject recipeCard;
    private GameObject recipe;

    private List<TMP_Text> allTexts;
    private List<TMP_Text> ingredientTexts;
    private TMP_Text recipeText;
    private TMP_Text ingredient1Text;
    private TMP_Text ingredient2Text;
    private TMP_Text ingredient3Text;
    private TMP_Text ingredient4Text;

    public bool isTutorial = false;
    public bool placedOnBlackboard = false;
    public bool retrievedFromBlackboard = false;
    private bool setTutorialCard = false;
    

    // Start is called before the first frame update
    void Start()
    {
        allTexts = new List<TMP_Text>();
        ingredientTexts = new List<TMP_Text>();
        currentQuestionIndex = 0;
        FindText();
        // •
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorial && !setTutorialCard)
		{
            SetTutorialCard();
            setTutorialCard = true;
		}
    }

    private void OnCollisionEnter(Collision collision)
	{
        if (isTutorial)
		{
            if (collision.gameObject.name.Equals("Recipe Card Holder"))
            {
                placedOnBlackboard = true;
            }
            else if (placedOnBlackboard && collision.gameObject.name.Equals("Drawers (1)"))
			{
                retrievedFromBlackboard = true;
			}
        }
	}

    private void FindText()
	{
        recipe = recipeCard.transform.Find("Recipe").gameObject;

        ingredient1Text = recipeCard.transform.Find("Ingredient 1").gameObject.GetComponent<TMP_Text>();
        ingredient2Text = recipeCard.transform.Find("Ingredient 2").gameObject.GetComponent<TMP_Text>();
        ingredient3Text = recipeCard.transform.Find("Ingredient 3").gameObject.GetComponent<TMP_Text>();
        ingredient4Text = recipeCard.transform.Find("Ingredient 4").gameObject.GetComponent<TMP_Text>();

        recipeText = recipe.GetComponent<TMP_Text>();
        allTexts.Add(ingredient1Text);
        allTexts.Add(ingredient2Text);
        allTexts.Add(ingredient3Text);
        allTexts.Add(ingredient4Text);
    }

    // Used in the tutorial as an example of a valid recipe card and layout
    public void SetTutorialCard()
	{
        if (recipe == null)
            FindText();

        recipeText.text = "Golf";

        allTexts[0].text = "• Golf Club";
        allTexts[1].text = "• Golf Ball";
        allTexts[2].text = "• Golf Cart";
        allTexts[3].text = "• Golf Tee";
    }

    public void ResetCard()
	{
        if (recipe == null)
            FindText();

        ingredientTexts.Clear();

        // Clear all card text first
        foreach (TMP_Text t in allTexts)
		{
            t.text = " ";
		}

        recipeText.text = " ";

        currentQuestionIndex = HellesKitchenManager.currentRound;
        
        currentQuestion = HellesKitchenManager.questionList[currentQuestionIndex];

        if (currentQuestion.answers.Length >= 1)
        {
            ingredientTexts.Add(ingredient1Text);
        }
        if (currentQuestion.answers.Length >= 2)
        {
            ingredientTexts.Add(ingredient2Text);
        }
        if (currentQuestion.answers.Length >= 3)
        {
            ingredientTexts.Add(ingredient3Text);
        }
        if (currentQuestion.answers.Length == 4)
        {
            ingredientTexts.Add(ingredient4Text);
        }

        currentQuestion = ELLEAPI.GetCustomQuestionsFromModule(menu.currentModule.moduleID)[currentQuestionIndex];

        if (currentQuestion == null)
            return;

        recipeText.text = currentQuestion.questionText;

        for (int i = 0; i < ingredientTexts.Count; i++)
        {
            ingredientTexts[i].text = "• " + currentQuestion.answers[i].front;
        } 
    }


}
