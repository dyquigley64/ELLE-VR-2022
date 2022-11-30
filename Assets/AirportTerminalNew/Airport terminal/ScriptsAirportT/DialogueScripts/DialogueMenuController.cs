using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueMenuController : MonoBehaviour
{
    public GameObject startBTN, exitBTN, infoCanvas;

    public Image startButton, nextButton, exitButton;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    public bool isAssistant = false;
    public DMAssistant assistantDialogueManager;
    public DMHostMom momDialogueManager;
    public DTAssistant assistantDialogueTrigger;
    public DTHostMom momDialogueTrigger;
    public Fader blackFader;

    private Color normalNext, highlightedNext, pressedNext, normalStart, highlightedStart, pressedStart;
    private bool nextHovered, startHovered, exitHovered, leftHandSelected, rightHandSelected;

    private AudioSource audio;
    public AudioClip click;

    void Start()
    {
        // Initialize colors for next button
        // #EFAC82 -> 239, 172, 130 -> 0.937f, 0.675f, 0.510f, 1.0f
        // #9B4904 -> 155, 73, 4 -> 0.601f, 0.286f, 0.016f, 1.0f
        // #422810 -> 66, 40, 16 -> 0.259f, 0.157f, 0.063f, 1.0f
        normalNext = new Color(0.937f, 0.675f, 0.510f, 1.0f);
        highlightedNext = new Color(0.601f, 0.286f, 0.016f, 1.0f);
        pressedNext = new Color(0.259f, 0.157f, 0.063f, 1.0f);

        // Initialize colors for start button
        // #462A11 -> 70, 42, 17 -> 0.275f, 0.165f, 0.067f, 1.0f
        // #C75050 -> 199, 80, 80 -> 0.780f, 0.314f, 0.314f, 1.0f
        // #686868 -> 104, 104, 104 -> 0.408f, 0.408f, 0.408f, 1.0f
        normalStart = new Color(0.275f, 0.165f, 0.067f, 1.0f);
        highlightedStart = new Color(0.780f, 0.314f, 0.314f, 1.0f);
        pressedStart = new Color(0.408f, 0.408f, 0.408f, 1.0f);

        audio = GetComponent<AudioSource>();

        if (nextButton != null)
        {
            nextButton.color = normalNext;
            startButton.color = normalStart;
        }
        else if (startButton != null)
        {
            startButton.color = normalNext;
        }
    }

    void Update()
    {
        nextHovered = startHovered = exitHovered = false;
        RaycastHit hit;

        leftHandSelected = !ELLEAPI.rightHanded;
        rightHandSelected = ELLEAPI.rightHanded;

        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHandSelected) // Check for left hand interactions
        {
            if (hit.transform.CompareTag("Menu"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "NextBTN")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select next.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        if (isAssistant)
                            assistantDialogueManager.DisplayNextSentence();
                        else if (momDialogueManager != null)
                            momDialogueManager.DisplayNextSentence();
                    }
                }
                if (hit.transform.name == "StartBTN")
                {
                    startHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select start.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        if (isAssistant)
                            assistantDialogueTrigger.TriggerDialogue();
                        else if (momDialogueTrigger != null)
                            momDialogueTrigger.TriggerDialogue();
                        else // Tutorial intro start button
                        {
                            startBTN.SetActive(false);
                            exitBTN.SetActive(false);
                            infoCanvas.SetActive(true);
                        }
                    }
                }
                if (hit.transform.name == "ExitBTN")
                {
                    exitHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select exit.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        // Exit tutorial and go to Bus scene
                        StartCoroutine(LoadBusScene());
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

        if (Physics.Raycast(rightHandPointer.position, rightHandPointer.forward, out hit) && rightHandSelected) // Check for right hand interactions
        {
            if (hit.transform.CompareTag("Menu"))
            {
                rightHandBeam.SetActive(true);
                rightHandDot.SetActive(true);
                rightHandDot.transform.eulerAngles = hit.normal;
                rightHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                rightHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "NextBTN")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select next.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        if (isAssistant)
                            assistantDialogueManager.DisplayNextSentence();
                        else if (momDialogueManager != null)
                            momDialogueManager.DisplayNextSentence();
                    }
                }
                if (hit.transform.name == "StartBTN")
                {
                    startHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select start.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        if (isAssistant)
                            assistantDialogueTrigger.TriggerDialogue();
                        else if (momDialogueTrigger != null)
                            momDialogueTrigger.TriggerDialogue();
                        else // Tutorial intro start button
                        {
                            startBTN.SetActive(false);
                            exitBTN.SetActive(false);
                            infoCanvas.SetActive(true);
                        }
                    }
                }
                if (hit.transform.name == "ExitBTN")
                {
                    exitHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select exit.");

                        if (audio != null) audio.PlayOneShot(click, 0.7f);

                        // Exit tutorial and go to Bus scene
                        StartCoroutine(LoadBusScene());
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

        if (nextButton != null)
        {
            if (nextHovered && (rightHandSelected && VRInput.rightTriggerDigitalDown) || (leftHandSelected && VRInput.leftTriggerDigitalDown))
                nextButton.color = pressedNext;
            else if (nextHovered)
                nextButton.color = highlightedNext;
            else
                nextButton.color = normalNext;

            if (startHovered && (rightHandSelected && VRInput.rightTriggerDigitalDown) || (leftHandSelected && VRInput.leftTriggerDigitalDown))
                startButton.color = pressedStart;
            else if (startHovered)
                startButton.color = highlightedStart;
            else
                startButton.color = normalStart;
        }
        else if (startButton != null)
        {
            if (startHovered && (rightHandSelected && VRInput.rightTriggerDigitalDown) || (leftHandSelected && VRInput.leftTriggerDigitalDown))
                startButton.color = pressedNext;
            else if (startHovered)
                startButton.color = highlightedNext;
            else
                startButton.color = normalNext;
        }

        if (!isAssistant)
        {
            if (exitHovered && (rightHandSelected && VRInput.rightTriggerDigitalDown) || (leftHandSelected && VRInput.leftTriggerDigitalDown))
                exitButton.color = pressedNext;
            else if (exitHovered)
                exitButton.color = highlightedNext;
            else
                exitButton.color = normalNext;
        }
    }

    private IEnumerator LoadBusScene()
    {
        //Debug.Log("starting fade");
        blackFader.Fade(true, 1f);
        //Debug.Log("starting wait coroutine");
        yield return new WaitForSeconds(2);
        //Debug.Log("finished wait coroutine, loading bus scene");
        SceneManager.LoadScene("Bus");
    }
}
