using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectorManager : MonoBehaviour
{
    //[SerializeField] int numberOfLocations;
    [SerializeField] GameObject[] presetLocations;
    [SerializeField] GameObject levelSelectMenu;
    //[SerializeField] string[] shirtColors;
    //[SerializeField] char[] genderDescriptors;
    

    [SerializeField] NonPlayableCharacter[] npcs;
    int locationDesitination;
    Hashtable dictionary = new Hashtable();

    bool houseHovered;
    bool marketHovered;
    public Fader blackFader;
    public Image houseButton, marketButton;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;

    // Start is called before the first frame update
    void Start()
    {
        //presetLocations = new GameObject[numberOfLocations];
        //npcs = new NonPlayableCharacter[numberOfLocations];
        SetLocation();
        dictionary.Add("Shirt", "camisa");
        dictionary.Add("Sweater", "suéter");
        dictionary.Add("Suit", "traje");
        dictionary.Add("Blue", "azul");
        dictionary.Add("Gray", "gris");
        dictionary.Add("White", "blanca"); //This only works with a shirt 
        dictionary.Add("Black", "negro"); //Works with suit/sweater
        dictionary.Add("Striped", "de rayas");
        dictionary.Add("Red", "roja"); //Only works with shirt

    }

    // Update is called once per frame
    void Update()
    {
        bool leftHandSelected, rightHandSelected;
        RaycastHit hit;

        leftHandSelected = !ELLEAPI.rightHanded;
        rightHandSelected = ELLEAPI.rightHanded;
        houseHovered = marketHovered = false;
        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHandSelected)
        {
            if (hit.transform.CompareTag("Menu"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "HouseButton")
                {
                    houseHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select house.");
                        StartCoroutine(GoToDestination("House"));
                    }
                }
                if (hit.transform.name == "MarketButton")
                {
                    marketHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select store.");
                        StartCoroutine(GoToDestination("Store"));
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

                if (hit.transform.name == "HouseButton")
                {
                    houseHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select house.");
                        StartCoroutine(GoToDestination("House"));
                    }
                }
                if (hit.transform.name == "MarketButton")
                {
                    marketHovered = false;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select store.");
                        StartCoroutine(GoToDestination("Store"));
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
        if(houseHovered)
            houseButton.color = Color.white;
        else
            houseButton.color = Color.white * 0.8f;
        
        if(marketHovered)
            marketButton.color = Color.white;
        else
            marketButton.color = Color.white * 0.8f;
    }

    public string getCommandText()
    {
        string result = $"Sientate al lado del la persona con la camisa {dictionary[npcs[locationDesitination].shirtColor.ToString()]}";

        levelSelectMenu.SetActive(true);

        return result;
    }

    public IEnumerator GoToDestination(string sceneName)
    {
        print("starting fade now");
        blackFader.Fade(true, 1f);
        print("starting wait now");
        yield return new WaitForSeconds(2);
        print("wait finished, now loading "+sceneName);
        SceneManager.LoadScene(sceneName);
    }
    //update button with pictures
    void SetLocation()
    {
        Random randomizer = new Random();
        int randMax = npcs.Length;
        locationDesitination = randomizer.Next(0, randMax);
        //print(presetLocations[locationDesitination]);
        levelSelectMenu.transform.position = presetLocations[locationDesitination].transform.position;
    }

    public String Translate()
    {
        string result = $"Sit next to the person with the {npcs[locationDesitination].shirtColor} {npcs[locationDesitination].top}";
        return result;
    }
}
