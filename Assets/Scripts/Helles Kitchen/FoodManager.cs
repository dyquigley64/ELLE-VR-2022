using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodManager : MonoBehaviour
{

    public HellesKitchenManager hellesKitchenManager;
    public List<Term> termsBag;

    [Header("Spawns food in random places")] [Tooltip("Does not despawn manually placed food already in level")]
    public bool spawnRandomFood = false;

    [Header("Maximum number of food objects to spawn in level (Per Individual Object)")]
    [Tooltip("Per Individual Object")]
    public int maxNumToSpawn = 6;

    [Header("Minimum number of food objects to spawn in level (Per Individual Object)")]
    [Tooltip("Per Individual Object")]
    public int minNumToSpawn = 1;

    [Header("The total number of food items the player will have to find")]
    public int numOfCorrectFood;

    public List<GameObject> foodObjectList = new List<GameObject>();
    public List<string> correctFoodNames = new List<string>();

    private List<string> foodList = new List<string>();
    private List<Vector3> coordinates = new List<Vector3>();

    // Start is called before the first frame update
   /* private void Start()
    {
        Debug.Log(hellesKitchenManager.spawnFood);
        if (hellesKitchenManager.spawnFood)
        {
            Debug.Log("In food manager");
            termsBag = hellesKitchenManager.termsBag;





            GameObject[] ingredientObjects = GameObject.FindGameObjectsWithTag("Food");

            GetNameOfTheFood();

            AddTermDataToLevel(ingredientObjects);

            //AddCorrectFoodToList();

            //GetRandomCoordinates(foodList.Count); // Moved into SpawnFoodInLevel() method

            // if (spawnRandomFood)
            // SpawnFoodInLevel();
        }

    }*/

    // Update is called once per frame
    void Update()
    {
    }

    private void GetNameOfTheFood()
    {
        foreach (var term in termsBag)
        {
            foodList.Add(term.front);
        }

        for (int j = 0; j < foodList.Count; j++)
        {
            Debug.Log("\n" + foodList[j]);

        }
        /*
        for (int i = 0; i < foodObjectList.Count; ++i)
        {
            foodList.Add(foodObjectList[i].name);
        }*/
    }

    private void AddTermDataToLevel(GameObject[] ingredientObjects)
    {
        

        char[] charToTrim = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(', ')', ' ' };
        int i = 0;
        //rename the gameobject to term
        /*
        foreach (GameObject gameObject in ingredientObjects)
        {

            
            if (gameObject.name.Trim(charToTrim) == "WrongIngredient")
            {
                gameObject.name = foodList[0]; // make sure this is the foreign term
                gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_Basemap", termsBag[0].image);//change the texture to the image
                i++;
            }
        }*/

        

        

    }

    private void AddCorrectFoodToList()
    {
        int rand = 0;

        char[] charToTrim = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(', ')', ' '};


        // Randomly assigns a food object as "correct" then adds that food object to the list
        for (int i = 0; i < numOfCorrectFood; ++i)
        {
            rand = Random.Range(0, foodList.Count);

            // This is to ensure that a food item can't be labeled "correct" more than once
            while (correctFoodNames.Contains(foodList[rand].Trim(charToTrim)))
            {
                rand = Random.Range(0, foodList.Count);
            }

            correctFoodNames.Add(foodList[rand].Trim(charToTrim));

            // If we want a food item to be "correct" more than once just use this line instead of the while loop.
            // Remember to spawn that food more than once as well!
            //correctFoodNames.Add(foodList[Random.Range(0, foodList.Count)]);
        }
    }

    // The param is the number of food items that we need to spawn in the game
    private void GetRandomCoordinates(int numOfCoordinates)
    {
        Vector3 coordinate;

        // Randomly gets the X position of the 3 shelfs from left to right facing them
        float xPosShelf1 = Random.Range(4f, 2.5f);
        float xPosShelf2 = Random.Range(1f, -.5f);
        float xPosShelf3 = Random.Range(-2f, -3.5f);

        // The Y position of the 3 shelfs from left to right. These are constants because they shouldn't be spawning in the air.
        const float yPosHighShelf = 2.016f;
        const float yPosMiddleShelf = 1.396f;
        const float yPosLowShelf = 0.778f;

        // This is an array of random x coordinates. Each index corresponds to 1 of the 3 shelves
        // This is so that we can randomly choose which of the three shelves to spawn the food on.
        float[] xCoordinates = {xPosShelf1, xPosShelf2, xPosShelf3};

        // This is an array of random y coords. Each index corresponds to the height level of a board on a shelf.
        float[] yCoordinates = {yPosHighShelf, yPosMiddleShelf, yPosLowShelf};

        // Now for each item of food we randomly choose a shelf and z-coord to spawn it on
        for (int i = 0; i < numOfCoordinates; ++i)
        {
            coordinate.x = xCoordinates[Random.Range(0, 3)];
            coordinate.y = yCoordinates[Random.Range(0, 3)];
            coordinate.z = Random.Range(-8.2f, -7.9f);

            coordinates.Add(coordinate);


            // This commented out code is where I tried to detect collisions, so that items don't spawn in each other. 
            /*if (coordinates[i].x == coordinate.x && coordinates[i].y == coordinate.y)
               continue;
            else
            {
                coordinates.Add(coordinate);
                ++i;
            }*/
        }
    }

    private void SpawnFoodInLevel()
    {
        int rand = 0;
        for (int i = 0; i < foodList.Count; ++i)
        {
            rand = Random.Range(minNumToSpawn, maxNumToSpawn);
            // Moves each item of food from outside the play area to in the play area
            //foodObjectList[i].GetComponent<Transform>().position = new Vector3(coordinates[i].x, coordinates[i].y, coordinates[i].z);

            // Creates a copy of the item of food in the play area
            for (int j = 0; j < rand; ++j)
            {
                //GetRandomCoordinates(foodList.Count);
                Instantiate(foodObjectList[i], new Vector3(coordinates[i].x, coordinates[i].y, coordinates[i].z),
                    foodObjectList[i].transform.rotation);
            }
        }
    }

    public void RemoveFoodFromKitchen(GameObject foodItem)
    {
        //foodItem.GetComponent<Transform>().position = new Vector3(2.1166f, 2.79f, 14.463f);

        // If instantiating the game objects use this line instead
        Destroy(foodItem);
    }
}