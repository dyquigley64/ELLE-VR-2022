using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BusDriver : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject commandCanvas;
    [SerializeField] TextMeshProUGUI canvasText;
    [SerializeField] TextMeshProUGUI translateText;

    private bool hovered, pressed = false;
    public Fader blackFader;
    public Image button;
    public GameObject buttonText;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GiveAction", levelSelect.GetComponent<LevelSelectorManager>().getCommandText());
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed) // Only checking for translate button, so no need to call this function after it's been pressed
            return;

        bool leftHandSelected, rightHandSelected;
        RaycastHit hit;

        leftHandSelected = !ELLEAPI.rightHanded;
        rightHandSelected = ELLEAPI.rightHanded;
        hovered = false;
        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHandSelected)
        {
            if (hit.transform.CompareTag("Menu"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "TranslateButton")
                {
                    hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select translate.");

                        translateText.text = levelSelect.GetComponent<LevelSelectorManager>().Translate();
                        button.enabled = false;
                        buttonText.SetActive(false);
                        pressed = true;
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

                if (hit.transform.name == "TranslateButton")
                {
                    hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select translate.");

                        translateText.text = levelSelect.GetComponent<LevelSelectorManager>().Translate();
                        button.enabled = false;
                        buttonText.SetActive(false);
                        pressed = true;
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
        if(hovered)
            button.color = Color.white;
        else
            button.color = Color.white * 0.8f;
    

    }
    IEnumerator GiveAction(string command)
    {
        yield return new WaitForSeconds(5);
        commandCanvas.SetActive(true);
        //commandCanvas.transform.GetChild(0).gameObject
        canvasText.text = command;
        //levelSelect.SetActive(true);
        button.gameObject.SetActive(true);
    }
}
