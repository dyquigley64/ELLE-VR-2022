using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialButtons : MonoBehaviour
{
    public Image StartButton, SkipButton01, SkipButton02, LeftHandedButton, RightHandedButton;
    public Image SkipContinousButton, ContinueButton, ReplayYesButton, ReplayNoButton;
    public Image NextButton01, NextButton02, NextButton03, NextButton04, NextButton05;
    public Image WarningNextButton, NextButton06, NextButton07, NextButton08, NextButton09;
    public Image FinishButton01, FinishButton02, FinishButton03, FinishButton04;
    public Image FinishButton05, FinishButton06, FinishButton07, FinishButton08, FinishButton09;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    private GameObject locomotionSystem;
    private AudioSource audio;
    public AudioClip button;

    public Transform leftHandPointer, rightHandPointer;

    public bool start = false;
    public bool leftHanded = true;
    public bool rightHanded = false;
    public bool buttonClicked = false;
    public bool next = false;
    public bool finish = false;
    public bool skipContinous = false;
    public bool playContinous = false;
    public bool replayYes = false;
    public bool replayNo = false;

    void Start()
    {
        locomotionSystem = GameObject.Find("Locomotion System");
        PlayerPrefs.SetInt("teleportMovement", 1);
        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = true;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool leftHandHovered, rightHandHovered, skipHovered, startHovered, continueHovered;
        bool skipContinousHovered, replayYesHovered, replayNoHovered, nextHovered, finishHovered;
        RaycastHit hit;

        leftHandHovered = rightHandHovered = skipHovered = startHovered = continueHovered = false;
        skipContinousHovered = replayYesHovered = replayNoHovered = nextHovered = finishHovered = false;
        leftHanded = !ELLEAPI.rightHanded;
        rightHanded = ELLEAPI.rightHanded;

        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHanded)
        {
            if (hit.transform.CompareTag("Tutorial Button"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "Start Button")
                {
                    startHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        start = true;
                    }
                }

                if (hit.transform.name == "Skip Button 01")
                {
                    skipHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        SceneManager.LoadScene("Hubworld");
                    }
                }

                if (hit.transform.name == "Skip Button 02")
                {
                    skipHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        //transform.GetComponent<HubworldPlayer>().chosenScene = "HubWorld";
                        SceneManager.LoadScene("Hubworld");
                    }
                }

                if (hit.transform.name == "Left-Handed Button")
                {
                    leftHandHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        leftHanded = true;
                        buttonClicked = true;
                        UpdateHand(false);
                    }
                }
                if (hit.transform.name == "Right-Handed Button")
                {
                    rightHandHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        rightHanded = true;
                        leftHanded = false;
                        buttonClicked = true;
                        UpdateHand(true);
                    }
                }
                if (hit.transform.name == "Skip Continous Button")
                {
                    skipContinousHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        skipContinous = true;
                    }
                }
                if (hit.transform.name == "Continue Button")
                {
                    continueHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        playContinous = true;
                        PlayerPrefs.SetInt("teleportMovement", 0);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = false;
                        Debug.Log("Used left hand to select continuous. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Replay Yes Button")
                {
                    replayYesHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        replayYes = true;
                        //transform.GetComponent<HubworldPlayer>().chosenScene = "MovementTutorial";
                        SceneManager.LoadScene("MovementTutorial");
                    }
                }
                if (hit.transform.name == "Replay No Button")
                {
                    replayNoHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        replayNo = true;
                    }
                }
                if (hit.transform.name == "Next Button 01")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 02")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 03")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 04")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 05")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Warning Next Button")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Next Button 06")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 07")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 08")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 09")
                {
                    nextHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Finish Button 01")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 02")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 03")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 04")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 05")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 06")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 07")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 08")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 09")
                {
                    finishHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
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

        if (Physics.Raycast(rightHandPointer.position, rightHandPointer.forward, out hit) && rightHanded)
        {
            if (hit.transform.CompareTag("Tutorial Button"))
            {
                rightHandBeam.SetActive(true);
                rightHandDot.SetActive(true);
                rightHandDot.transform.eulerAngles = hit.normal;
                rightHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                rightHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "Start Button")
                {
                    startHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        start = true;
                    }
                }

                if (hit.transform.name == "Skip Button 01")
                {
                    skipHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        SceneManager.LoadScene("HubWorld");
                    }
                }

                if (hit.transform.name == "Skip Button 02")
                {
                    skipHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        SceneManager.LoadScene("HubWorld");
                    }
                }

                if (hit.transform.name == "Left-Handed Button")
                {
                    leftHandHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        leftHanded = true;
                        buttonClicked = true;
                        UpdateHand(false);
                    }
                }
                if (hit.transform.name == "Right-Handed Button")
                {
                    rightHandHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        rightHanded = true;
                        leftHanded = false;
                        buttonClicked = true;
                        UpdateHand(true);
                    }
                }
                if (hit.transform.name == "Skip Continous Button")
                {
                    skipContinousHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        skipContinous = true;
                    }
                }
                if (hit.transform.name == "Replay Yes Button")
                {
                    replayYesHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        replayYes = true;
                        SceneManager.LoadScene("MovementTutorial");
                    }
                }
                if (hit.transform.name == "Replay No Button")
                {
                    replayNoHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        replayNo = true;
                    }
                }
                if (hit.transform.name == "Continue Button")
                {
                    continueHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        playContinous = true;
                        PlayerPrefs.SetInt("teleportMovement", 0);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = false;
                        Debug.Log("Used left hand to select continuous. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Next Button 01")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 02")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 03")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 04")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 05")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Warning Next Button")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Next Button 06")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 07")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 08")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Next Button 09")
                {
                    nextHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        next = true;
                    }
                }
                if (hit.transform.name == "Finish Button 01")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 02")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 03")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 04")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 05")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 06")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 07")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 08")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
                    }
                }
                if (hit.transform.name == "Finish Button 09")
                {
                    finishHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        audio.PlayOneShot(button);
                        finish = true;
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

        if (startHovered)
            StartButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            StartButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (skipHovered)
            SkipButton01.color = SkipButton02.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
           SkipButton01.color = SkipButton02.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (leftHandHovered)
            LeftHandedButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            LeftHandedButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (rightHandHovered)
            RightHandedButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            RightHandedButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (skipContinousHovered)
            SkipContinousButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            SkipContinousButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (continueHovered)
            ContinueButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else 
            ContinueButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (replayYesHovered)
            ReplayYesButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            ReplayYesButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (replayNoHovered)
            ReplayNoButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        else
            ReplayNoButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);

        if (nextHovered)
        {
            NextButton01.color = NextButton02.color = NextButton03.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
            NextButton04.color = NextButton05.color = NextButton06.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
            NextButton07.color = NextButton08.color = NextButton09.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
            WarningNextButton.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        }
        else
        {
            NextButton01.color = NextButton02.color = NextButton03.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
            NextButton04.color = NextButton05.color = NextButton06.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
            NextButton07.color = NextButton08.color = NextButton09.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
            WarningNextButton.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
        }

        if (finishHovered)
        {
            FinishButton01.color = FinishButton02.color = FinishButton03.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
            FinishButton04.color = FinishButton05.color = FinishButton06.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
            FinishButton07.color = FinishButton08.color = FinishButton09.color = new Color(0.6320754f, 0.605242f, 0.605242f, 1f);
        }
        else
        {
            FinishButton01.color = FinishButton02.color = FinishButton03.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
            FinishButton04.color = FinishButton05.color = FinishButton06.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
            FinishButton07.color = FinishButton08.color = FinishButton09.color = new Color(1f, 0.9575472f, 0.9575472f, 1f);
        }

    }

    void UpdateHand(bool rightHandSelected)
    {
        ELLEAPI.rightHanded = rightHandSelected;
        ELLEAPI.UpdatePreferences();
    }
}
