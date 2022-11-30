using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static System.Random;

public class ExitMenuController : MonoBehaviour
{
    public Image exitButton; // Reset -> Exit
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    public MenuController Menu;

    private GameObject[] ObjectsGathered;
    public int numObjectsToGather;
    private int numObjects;
    public GameObject[] ObjectsNeeded;
    private GameObject[] Objects;
    public DepositController Deposit;
    [SerializeField] private MillelleManager manager;
    void Start()
    {
        Objects = Menu.Objects;
        exitButton.color = Color.white * .9f;
        numObjects = Objects.Length;
        ObjectsGathered = new GameObject[numObjects];
        ObjectsNeeded = GetObjectsNeeded();
        manager.TrimTerms(ObjectsNeeded);
    }

    void Update()
    {
        bool leftHandHovered, rightHandHovered, exitHovered, unpackHovered, logoutHovered;
        bool leftHandSelected, rightHandSelected, exitSelected;
        RaycastHit hit;

        exitHovered = false;
        leftHandSelected = !ELLEAPI.rightHanded;
        rightHandSelected = ELLEAPI.rightHanded;

        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHandSelected)
        {
            if (hit.transform.CompareTag("Menu"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "Exit Button")
                {
                    exitHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select exit.");
                        SubmitAndExit();
                    }
                }
            }
            else
            {
                leftHandBeam.SetActive(false);
                leftHandDot.SetActive(false);
            }
        }
        else
        {
            leftHandBeam.SetActive(false);
            leftHandDot.SetActive(false);
        }

        if (Physics.Raycast(rightHandPointer.position, rightHandPointer.forward, out hit) && rightHandSelected)
        {
            if (hit.transform.CompareTag("Menu"))
            {
                rightHandBeam.SetActive(true);
                rightHandDot.SetActive(true);
                rightHandDot.transform.eulerAngles = hit.normal;
                rightHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                rightHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "Exit Button")
                {
                    exitHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select exit.");
                        SubmitAndExit();
                    }
                }
            }
            else
            {
                rightHandBeam.SetActive(false);
                rightHandDot.SetActive(false);
            }
        }
        else
        {
            rightHandBeam.SetActive(false);
            rightHandDot.SetActive(false);
        }

        if (exitHovered)
            exitButton.color = Color.white;
        else
            exitButton.color = Color.white * 0.8f;
    }

    // Randomly choose objects throughout the scene to assign to the player
    GameObject[] GetObjectsNeeded()
    {
        GameObject[] ObjectsNeeded = new GameObject[numObjectsToGather];
        int[] Indexes = new int[numObjectsToGather];
        System.Random rand = new System.Random();
        int index = rand.Next(numObjects); // Random.Next(upperBound) is not inclusive, so this should be fine

        /* // Remove comment if next for loop breaks
        for (int i = 0; i < numObjectsToGather; i++)
        {
            while (Indexes.Exists(index)) index = rand.Next(numObjects);
            Indexes[i] = index;
        }
        */

        for (int i = 0; i < numObjectsToGather; i++)
        {
            while (Array.Exists(Indexes, i => i == index)) index = rand.Next(numObjects);
            Indexes[i] = index;

            ObjectsNeeded[i] = Objects[Indexes[i]];
        }

        return ObjectsNeeded;
    }

    // Submits the player's gathered objects for scoring and returns them to the bus scene
    void SubmitAndExit()
    {
        /*
        int i = 0;
        while (Deposit.objCount > 0)
        {
            ObjectsGathered[i] = Deposit.DepositedObjects.Pop();
            i++;
        }
        */

        int score = Score(Deposit.DepositedObjects);

        print("Score is " + score);

        manager.EndGame(score, ObjectsNeeded, Deposit.DepositedObjects);
    }

    // Check the player's gathered items against the needed objects list and return their score out of 100
    private int Score(GameObject[] ObjectsGathered)
    {
        float score = (float)Math.Min(Deposit.objCount, ObjectsNeeded.Length);

        foreach (GameObject i in ObjectsGathered)
        {
            if (i == null) continue;
            
            if (!Array.Exists(ObjectsNeeded, obj => obj == i)) // Wrong item gathered
                score -= 1.0f;

            if (Array.Exists(ObjectsNeeded, obj => obj == i) && i.GetComponent<ObjectController>().hintUsed) // Right item gathered, but hint used
                score -= 0.5f;
            
            if (score <= 0) // Minimum score reached, no need to continue
                return 0;
        }

        /* // This loop is handled by the initialization for score
        foreach (GameObject i in ObjectsNeeded)
        {
            if (!Array.Exists(ObjectsGathered, obj => obj == i)) // Missing a correct item
                score--;
            
            if (score == 0) // Minimum score reached, no need to continue
                return score;
        }
        */

        return (int)((score / (float)ObjectsNeeded.Length) * 100f);
    }
}
