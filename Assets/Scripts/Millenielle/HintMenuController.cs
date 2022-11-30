using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintMenuController : MonoBehaviour
{
    public Image[] hintButtons;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;

    public TextMeshProUGUI[] objectNames, buttonTexts;
    public GameObject[] buttonObjects;
    public ExitMenuController ExitMenu;

    private AudioSource audio;
    public AudioClip hintSound;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        foreach (Image img in hintButtons)
        {
            img.color = Color.white * .6f;
        }
        StartCoroutine(InitializeNames(0.1f));
    }

    void Update()
    {
        bool hint1Hovered, hint2Hovered, hint3Hovered;
        bool leftHandSelected, rightHandSelected;
        RaycastHit hit;

        hint1Hovered = hint2Hovered = hint3Hovered = false;
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

                if (hit.transform.name == "Item 1 Hint Button")
                {
                    hint1Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select hint 1.");
                        hint(0);
                    }
                }
                if (hit.transform.name == "Item 2 Hint Button")
                {
                    hint2Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select hint 2.");
                        hint(1);
                    }
                }
                if (hit.transform.name == "Item 3 Hint Button")
                {
                    hint3Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select hint 3.");
                        hint(2);
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

                if (hit.transform.name == "Item 1 Hint Button")
                {
                    hint1Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select hint 1.");
                        hint(0);
                    }
                }
                if (hit.transform.name == "Item 2 Hint Button")
                {
                    hint2Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select hint 2.");
                        hint(1);
                    }
                }
                if (hit.transform.name == "Item 3 Hint Button")
                {
                    hint3Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select hint 3.");
                        hint(2);
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

        if (hint1Hovered)
            hintButtons[0].color = Color.white;
        else
            hintButtons[0].color = Color.white * 0.8f;

        if (hint2Hovered)
            hintButtons[1].color = Color.white;
        else
            hintButtons[1].color = Color.white * 0.8f;

        if (hint3Hovered)
            hintButtons[2].color = Color.white;
        else
            hintButtons[2].color = Color.white * 0.8f;
    }

    // Reveals the hint for a given object
    void hint(int index)
    {
        if (index < 0 || index > 2)
            return;
        
        if (audio != null) audio.PlayOneShot(hintSound, 0.7f);
        
        buttonObjects[index].SetActive(false);
        buttonTexts[index].SetText(ExitMenu.ObjectsNeeded[index].GetComponent<ObjectController>().objectHint);
        ExitMenu.ObjectsNeeded[index].GetComponent<ObjectController>().hintUsed = true;
    }

    // Initialize the object names after waiting t seconds
    IEnumerator InitializeNames(float t)
    {
        yield return new WaitForSeconds(t);
        
        for (int i = 0; i < 3; i++)
        {
            string objName = ExitMenu.ObjectsNeeded[i].GetComponent<ObjectController>().spanishName;

            if (string.IsNullOrEmpty(objName)) objName = "OBJECT NOT NAMED"; // Check if object being added has a name assigned

            objectNames[i].SetText(objName);
        }
    }
}
