using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject WelcomeCanvas, IntroCanvas, DominantHandCanvas, LearnCameraCanvas; 
    public GameObject StartCanvas, WarningCanvas, TutorialIntroCanvas, FirstCanvas;
    public GameObject FirstContinousCanvas, FinishContinousCanvas;
    public GameObject FirstTeleportCanvas, FinishTeleportCanvas, SkipTeleportCanvas;
    public GameObject ReplayCanvas, FinishCanvas;
    public GameObject StartGoal, FirstContinousGoal, SecondContinousGoal, SwitchGoal;
    public GameObject FirstTeleportGoal, FinishGoal;
    public GameObject NextButton01, NextButton02, NextButton03, NextButton04, NextButton05;
    public GameObject WarningNextButton, NextButton06, NextButton07, NextButton08, NextButton09;
    public GameObject FinishButton01, FinishButton02, FinishButton03, FinishButton04;
    public GameObject FinishButton05, FinishButton06, FinishButton07, FinishButton08, FinishButton09;
    public GameObject Coin01, Coin02, Coin03, Coin04, Coin05, Coin06, Coin07, Coin08, Coin09;
    public GameObject Coin10, Coin11, Coin12, Coin13, Coin14, Coin15, Coin16, Coin17;
    public GameObject Barrier01, Barrier02, Barrier03, Barrier04, Barrier05, Barrier06, Barrier07;
    public GameObject LocomotionSystem;

    public TutorialButtons tutorialButtons;

    public TMP_Text welcomeCanvasText;
    public TMP_Text introCanvasText;
    public TMP_Text learnCameraText;
    public TMP_Text startCanvasText;
    public TMP_Text warnCanvasText;
    public TMP_Text tutorialIntroText;
    public TMP_Text firstCanvasText;
    public TMP_Text firstContinousCanvasText;
    public TMP_Text finishContinousCanvasText;
    public TMP_Text firstTeleportCanvasText;
    public TMP_Text finishTeleportCanvasText;
    public TMP_Text skipTeleportText;
    public TMP_Text replayCanvasText;
    public TMP_Text finishCanvasText;

    public int hand = 0;
    public int movementType = 1;
    public int goalCount = 0;
    public int index;
    public int last = 0;

    public string text;
    public List<string> instructions = new List<string>();

    private AudioSource audio;
    public AudioClip canvasWoosh;

    /*
     * Welcome
     * What you'll be doing today
     * Setup hands
     * teach buttons
     * Teach camera
     * Direct to goal
     * Teach teleport movement controls
     * Teach continous movement controls
     * Teach minigame instructions
     */

    void Start()
    {
        // Although we can SetActive(false) in editor, we set them here in code for clarity
        DominantHandCanvas.SetActive(false);
        FirstTeleportCanvas.SetActive(false);
        FinishTeleportCanvas.SetActive(false);
        StartCanvas.SetActive(false);
        TutorialIntroCanvas.SetActive(false);
        FirstCanvas.SetActive(false);
        FinishCanvas.SetActive(false);
        IntroCanvas.SetActive(false);
        WarningCanvas.SetActive(false);
        LearnCameraCanvas.SetActive(false);
        FirstContinousCanvas.SetActive(false);
        FinishContinousCanvas.SetActive(false);

        StartGoal.SetActive(false);
        FirstContinousGoal.SetActive(false);
        SecondContinousGoal.SetActive(false);
        SwitchGoal.SetActive(false);
        FirstTeleportGoal.SetActive(false);
        SkipTeleportCanvas.SetActive(false);
        ReplayCanvas.SetActive(false);
        FinishGoal.SetActive(false);

        Barrier01.SetActive(false);
        Barrier02.SetActive(false);
        Barrier03.SetActive(false);
        Barrier04.SetActive(false);
        Barrier05.SetActive(false);
        //Barrier06.SetActive(false);
        //Barrier07.SetActive(false);

        FinishButton01.SetActive(false);
        FinishButton02.SetActive(false);
        FinishButton03.SetActive(false);
        FinishButton04.SetActive(false);
        FinishButton05.SetActive(false);
        FinishButton06.SetActive(false);
        FinishButton07.SetActive(false);
        FinishButton08.SetActive(false);
        FinishButton09.SetActive(false);

        Coin01.SetActive(false);
        Coin02.SetActive(false);
        Coin03.SetActive(false);
        Coin04.SetActive(false);
        Coin05.SetActive(false);
        Coin06.SetActive(false);
        Coin07.SetActive(false);
        Coin08.SetActive(false);
        Coin09.SetActive(false);
        Coin10.SetActive(false);
        Coin11.SetActive(false);
        Coin12.SetActive(false);
        Coin13.SetActive(false);
        Coin14.SetActive(false);
        Coin15.SetActive(false);
        Coin16.SetActive(false);
        Coin17.SetActive(false);

        audio = GetComponent<AudioSource>();

        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        StartCoroutine(WelcomeRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Welcome */
    // --------------------------------------------------------------------
    IEnumerator WelcomeRoutine()
    {
        WelcomeCanvas.SetActive(true);
        welcomeCanvasText.text = "Welcome to ELLE!  The language game hub!";
        yield return new WaitForSeconds(6);
        welcomeCanvasText.text = "Let's learn how to play in the hubworld";
        yield return new WaitForSeconds(6);
        WelcomeCanvas.SetActive(false);
        StartCoroutine(IntroCanvasRoutine());
    }  
    // --------------------------------------------------------------------

    /* Learn Buttons */
    // --------------------------------------------------------------------
    IEnumerator IntroCanvasRoutine()
    {
        IntroCanvas.SetActive(true);
        introCanvasText.text = "Point and click the trigger to select buttons";

        while (tutorialButtons.start == false)
        {
            //yield return new WaitForSeconds(1);
            yield return null;
        }

        IntroCanvas.SetActive(false);
        StartCoroutine(SetupHands());
    }
    // --------------------------------------------------------------------

    /* Setup hands */
    // --------------------------------------------------------------------
    IEnumerator SetupHands()
    {
        DominantHandCanvas.SetActive(true);

        while (tutorialButtons.buttonClicked == false)
        {
            //yield return new WaitForSeconds(1);
            yield return null;
        }

        // By default hand = 0, which is left
        int handFlag = 1;
        while (handFlag == 1)         
        {
            if (tutorialButtons.rightHanded == true)
            {
                hand = 1;
                handFlag = 0;
                tutorialButtons.rightHanded = false;
            }
            if (tutorialButtons.leftHanded == true)
            {
                hand = 0;
                handFlag = 0;
                tutorialButtons.leftHanded = false;
            }
            yield return null;
        }

        tutorialButtons.buttonClicked = false;
        DominantHandCanvas.SetActive(false);
        StartCoroutine(TeachCamera(hand));
    }
    // --------------------------------------------------------------------
      
    /* Teach Camera */
    // --------------------------------------------------------------------
    IEnumerator TeachCamera(int hand)
    {
        /* if right and left
         * Your donminant(R or L) hand controls the camera
         * Move the joystick to shift the camera
         * Look to the left to start the game!
         */
        
        //yield return new WaitForSeconds(1);

        LearnCameraCanvas.SetActive(true);

        int learnCameraFlag = 1;
        while (learnCameraFlag == 1)
        {
            if (hand == 0)
            {
                learnCameraText.text = "Your left hand controls the camera";
                learnCameraFlag = 0;
            }
            if (hand == 1)
            {
                learnCameraText.text = "Your right hand controls the camera";
                learnCameraFlag = 0;
            }
            yield return null;
        }

        instructions.Add("Move the joystick to shift the camera");
        instructions.Add("Look to your left to start the game!");

        int cameraFlag = 1;
        while (cameraFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    learnCameraText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton01.SetActive(false);
                FinishButton01.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                tutorialButtons.finish = false;
                cameraFlag = 0;
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("learnCameraText.text", index));
        //yield return new WaitForSeconds(3);
        instructions.Clear();
        index = 0;
        LearnCameraCanvas.SetActive(false);
        StartCoroutine(StartTutorial(hand));
    }
    // --------------------------------------------------------------------

    /* Start tutorial */
    // --------------------------------------------------------------------
    IEnumerator StartTutorial(int hand)
    {
        //Let's learn how to move in the elle boardgame
        //  We will learn two ways to move on the way

       // yield return new WaitForSeconds(1);
        StartCanvas.SetActive(true);
        audio.PlayOneShot(canvasWoosh);

        //startCanvasText.text = "Let's learn how to move in the elle boardgame";
        startCanvasText.text = "Let's learn how to move in ELLE";
        //instructions.Add("We will learn two ways to move on the way");
        instructions.Add("We will learn two ways to move");
        instructions.Add("Warning: \n Continous movement may cause motion sickness");

        int startFlag = 3;
        while (startFlag > 0)
        {
           /* while (index < instructions.Count)
            {
                if (tutorialButtons.finish == true)
                {
                    text = instructions[index];
                    startCanvasText.text = text;
                    index++;
                    tutorialButtons.finish = false;
                    startFlag--;
                    Debug.Log("startFlag = " + startFlag);
                    yield return new WaitForSeconds(1);
                }
                yield return null;
            }*/

            if (tutorialButtons.finish == true)
            {
                if (index < instructions.Count)
                {
                    text = instructions[index];
                    startCanvasText.text = text;
                }
                index++;
                tutorialButtons.finish = false;
                startFlag--;
                Debug.Log("startFlag = " + startFlag);
                yield return new WaitForSeconds(1);
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("startCanvasText.text", index));
        //yield return new WaitForSeconds(3);
        instructions.Clear();
        index = 0;
        StartCanvas.SetActive(false);
        StartCoroutine(WarnCanvas(hand));
    }
    // --------------------------------------------------------------------

    /* Motion Sickness warning */
    // --------------------------------------------------------------------
    IEnumerator WarnCanvas(int hand)
    {
        // Warning:      continous movement may cause motion sickness
        /* To skip to teleportation movement
        Click skip*/
        Debug.Log("Start");
        //yield return new WaitForSeconds(1);
        WarningCanvas.SetActive(true);

        warnCanvasText.text = "To skip to teleportation movement:\nClick skip";

        int warnContinous = 1;
        while (warnContinous == 1)
        {
            if (tutorialButtons.playContinous == true)
            {
                // By default movementType = 1, which is teleportation 
                movementType = 0;
                WarningCanvas.SetActive(false);
                tutorialButtons.playContinous = false;
                warnContinous = 0;
                //StartCoroutine(TutorialIntro(hand, movementType));
                
                yield return null;
            }

            if (tutorialButtons.skipContinous == true)
            {
                movementType = 1;
                WarningCanvas.SetActive(false);
                tutorialButtons.skipContinous = false;
                warnContinous = 0;
                //StartCoroutine(TutorialIntro(hand, movementType));
                
                yield return null;
            }
            
            yield return null;
        }
        //yield return new WaitForSeconds(1);
        StartCoroutine(TutorialIntro(hand, movementType));
    }
    // --------------------------------------------------------------------

    /* Intro to tutorial */
    // --------------------------------------------------------------------
    IEnumerator TutorialIntro(int hand, int movementType)
    {
        /*
         * Let's move along the Path!
         * 
         * Continous: Your non-dominant hand controls movement
         * 
         * Teleport: Hold the joystick forward and aim for a spot
         *          Release the joystick to teleport
         * SPawn first goal
         * Use what you learned to
         * move to the first goal
         * move to the blue space
         * unfreeze movement
         */

        //yield return new WaitForSeconds(1);
        TutorialIntroCanvas.SetActive(true);
    
        tutorialIntroText.text = "Let's move along the Path!";
        
        int movementFlag = 1;
        while (movementFlag == 1)
        {
            if (movementType == 1)
            {
                instructions.Add("Hold the joystick forward and aim for a spot");
                instructions.Add("Release the joystick to teleport");
                movementFlag = 0;
            }
            if (movementType == 0)
            {
                if (hand == 0)
                {
                    instructions.Add("Your right hand controls movement");
                    movementFlag = 0;
                }
                if (hand == 1)
                {
                    instructions.Add("Your left hand controls movement");
                    movementFlag = 0;
                }
            }
            yield return null;
       }

        instructions.Add("Try to move on to the blue space");

        int introFlag = 1;
        while (introFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    tutorialIntroText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton02.SetActive(false);
                FinishButton02.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                StartGoal.SetActive(true);
                TutorialIntroCanvas.SetActive(false);
                tutorialButtons.finish = false;
                introFlag = 0;
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("tutorialIntroText.text", index)); 
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();
        //yield return new WaitForSeconds(5);
        
        int firstGoalFlag = 1;
        while (firstGoalFlag == 1)
        {
            if (goalCount == 1)
            {
                instructions.Clear();
                index = 0;
                firstGoalFlag = 0;
                StartCoroutine(FirstCanvasRoutine(hand, movementType));
            }
            yield return null;
        }
    }
     // -------------------------------------------------------------------- 

    /* Get player to the first goals */
    // -------------------------------------------------------------------- 
    IEnumerator FirstCanvasRoutine(int hand, int movementType)
    {
        /* Teleport: Let's get moving
         * Jump over the hurdles!
         * Can you make it to the blue space?
         * 
         * Continous: Let's get   moving!
         * Can you make it to the next blue space?
         */

        //yield return new WaitForSeconds(1);
        FirstCanvas.SetActive(true);
        audio.PlayOneShot(canvasWoosh);
        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();

        //int firstCanvasRoutineFlag = 1;
        //while (firstCanvasRoutineFlag == 1)
        //{
        if (movementType == 1)
            {
                firstCanvasText.text = "Let's get moving!";
                //instructions.Add("Jump over the hurdle!");
                instructions.Add("Time for a bigger jump!");
                instructions.Add("Can you make it to the blue space?");
                //firstCanvasRoutineFlag = 0;
                SwitchGoal.SetActive(true);
                Coin13.SetActive(true);
                Coin14.SetActive(true);
                Coin15.SetActive(true);
                Coin16.SetActive(true);
                Coin17.SetActive(true);
                //Barrier01.SetActive(true);
            }
            else
            {
                firstCanvasText.text = "Let's get moving!";
                instructions.Add("Can you make it to the next blue space?");
                //firstCanvasRoutineFlag = 0;
                FirstContinousGoal.SetActive(true);
                Coin01.SetActive(true);
                Coin02.SetActive(true);
                Coin03.SetActive(true);
                Coin04.SetActive(true);
                Coin05.SetActive(true);
                Coin06.SetActive(true);
                Coin07.SetActive(true);
                Coin08.SetActive(true);
                Coin09.SetActive(true);
                Coin10.SetActive(true);
                //Coin11.SetActive(true);
                Coin12.SetActive(true);
                Coin13.SetActive(true);
                Coin14.SetActive(true);
                Coin15.SetActive(true);
                Coin16.SetActive(true);
                Coin17.SetActive(true);
        }
            
            yield return null;
        //}

        int firstCanvasFlag = 1;
        while (firstCanvasFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    firstCanvasText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton03.SetActive(false);
                FinishButton03.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                FirstCanvas.SetActive(false);
                tutorialButtons.finish = false;
                firstCanvasFlag = 0;
            }

            yield return null;
        }
        
        //yield return new WaitForSeconds(5);
        //yield return StartCoroutine(PrintInstructions("firstCanvasText.text", index));
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        int secondGoalFlag = 1;
        while (secondGoalFlag == 1)
        {
            if (goalCount == 2)
            {
                instructions.Clear();
                index = 0;
                secondGoalFlag = 0;

                if (movementType == 1)
                {
                    StartCoroutine(FirstTeleport(hand, movementType));
                }
                else
                {

                    StartCoroutine(FirstContinous(hand, movementType));
                }
            }
            yield return null;
        }
    }

    /* Get player to the second continous goal */
    // -------------------------------------------------------------------- 
    IEnumerator FirstContinous(int hand, int movementType)
    {
        /*
         * You made it!
         * Oh no! the couch is in the way!
         * Try to find the next blue space
         * Maybe following the path will help...
         */

        //yield return new WaitForSeconds(1);
        FirstContinousCanvas.SetActive(true);
        audio.PlayOneShot(canvasWoosh);
        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        SecondContinousGoal.SetActive(true);

        firstContinousCanvasText.text = "You made it!";
        instructions.Add("Where's the next blue space?");
        instructions.Add("Oh no! the couch is in the way!");
        instructions.Add("Maybe following the path will help...");

        int firstContinousFlag = 1;
        while (firstContinousFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    firstContinousCanvasText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton04.SetActive(false);
                FinishButton04.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                FirstContinousCanvas.SetActive(false);
                tutorialButtons.finish = false;
                firstContinousFlag = 0;
            }

            yield return null;
        }

        //yield return new WaitForSeconds(5);
        //yield return StartCoroutine(PrintInstructions("firstContinousCanvasText.text", index));
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        int thirdGoalFlag = 1;
        while (thirdGoalFlag == 1)
        {
            if (goalCount == 3)
            {
                instructions.Clear();
                index = 0;
                thirdGoalFlag = 0;
                StartCoroutine(FinishContinous(hand, movementType));
            }
            yield return null;
        }
    }
    // -------------------------------------------------------------------- 

    /* Get player to last goal of continous movement */
    // -------------------------------------------------------------------- 
    IEnumerator FinishContinous(int hand, int movementType)
    {   /*
        Great job!
         The next blue space is special!
         Let's find out why...
        */

        //yield return new WaitForSeconds(1);
        FinishContinousCanvas.SetActive(true);
        audio.PlayOneShot(canvasWoosh);
        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        SwitchGoal.SetActive(true);

        finishContinousCanvasText.text = "Great job!";
        instructions.Add("The next blue space is special!");
        instructions.Add("Go and find out why...");

        int finishContinousFlag = 1;
        while (finishContinousFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    finishContinousCanvasText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton05.SetActive(false);
                FinishButton05.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                FinishContinousCanvas.SetActive(false);
                tutorialButtons.finish = false;
                finishContinousFlag = 0;
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("finishContinousCanvasText.text", index));
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        int switchFlag = 1;
        while (switchFlag == 1)
        {
            if (goalCount == 4)
            {
                instructions.Clear();
                index = 0;
                switchFlag = 0;
                StartCoroutine(FirstTeleport(hand, movementType));
            }
            
            yield return null;
        }
    }
    // --------------------------------------------------------------------

    /* Switch from continous to teleportation */
    /* Players first goal for teleportation */
    // -------------------------------------------------------------------- 
    IEnumerator FirstTeleport(int hand, int movementType)
    {
        // Teleport:
        // Nice jump!
        // Now use your new teleport skills!
        // Clear all of the hurdles along the path
        // Continous:
        // We are going to learn... 
        // how to teleport!
        // Hold the joystick forward and aim for a spot
        // Release the joystick to teleport
        // Use your camera to find the next blue space

        //yield return new WaitForSeconds(1);
        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();

        FirstTeleportGoal.SetActive(true);
        audio.PlayOneShot(canvasWoosh);

        int firstTFlag = 1;
        while (firstTFlag == 1)
        {
            if (movementType == 1)
            {
                SkipTeleportCanvas.SetActive(true);
                skipTeleportText.text = "Nice jump!";
                instructions.Add("Time to use your new teleport skills!");
                instructions.Add("Use your camera to find the next blue space");
                instructions.Add("Jump over the hurdle!");
                //instructions.Add("Clear all of the hurdles along the path");
                firstTFlag = 0;
                Barrier02.SetActive(true);
            }
            else
            {
                FirstTeleportCanvas.SetActive(true);
                //firstTeleportCanvasText.text = "We are going to learn... ";
                //instructions.Add("how to teleport!");
                firstTeleportCanvasText.text = "We are going to learn how to teleport!";
                instructions.Add("Hold the joystick forward and aim for a spot");
                instructions.Add("Release the joystick to teleport");
                instructions.Add("Can you make it to the next blue space?");

                PlayerPrefs.SetInt("teleportMovement", 1);
                PlayerPrefs.SetInt("movementEnabled", 0);
                //LocomotionSystem.GetComponent<TeleportationManager>().movementEnabled = false;
                LocomotionSystem.GetComponent<TeleportationManager>().teleportMovement = true;
                yield return new WaitForSeconds(1);
                PlayerPrefs.SetInt("movementEnabled", 1);
                //LocomotionSystem.GetComponent<TeleportationManager>().movementEnabled = true;
                firstTFlag = 0;
            }
            
            yield return null;
        }

        int firstTeleportFlag = 1;
        while (firstTeleportFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    if (movementType == 1)
                    {
                        text = instructions[index];
                        skipTeleportText.text = text;
                        index++;
                        tutorialButtons.next = false;
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        text = instructions[index];
                        firstTeleportCanvasText.text = text;
                        index++;
                        tutorialButtons.next = false;
                        yield return new WaitForSeconds(1);
                    }
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                if (movementType == 1)
                {
                    NextButton06.SetActive(false);
                    FinishButton06.SetActive(true);
                    last = 0;
                }
                else
                {
                    NextButton07.SetActive(false);
                    FinishButton07.SetActive(true);
                    last = 0;
                }
            }

            if (tutorialButtons.finish == true)
            {
                if (movementType == 1)
                {
                    SkipTeleportCanvas.SetActive(false);
                    tutorialButtons.finish = false;
                    firstTeleportFlag = 0;
                }
                else
                {
                    FirstTeleportCanvas.SetActive(false);
                    tutorialButtons.finish = false;
                    firstTeleportFlag = 0;
                }
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("firstTeleportCanvasText.text", index));
        // LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        int fifthGoalFlag = 1;
        while (fifthGoalFlag == 1)
        {           
            //if (goalCount == 5)
            if (goalCount == 5 || goalCount == 3)
            {
                instructions.Clear();
                index = 0;
                fifthGoalFlag = 0;
                StartCoroutine(FinishTeleport(hand, movementType));
            }
            /*
            else
            {
                instructions.Clear();
                index = 0;
                fifthGoalFlag = 0;
                StartCoroutine(FinishTeleport(hand, movementType));
            }
            */
            yield return null;
        }
    }
    // --------------------------------------------------------------------

    /* Player reached second teleportation goal */
    // --------------------------------------------------------------------
    IEnumerator FinishTeleport(int hand, int movementType)
    {
        //yield return new WaitForSeconds(1);
        FinishTeleportCanvas.SetActive(true);

        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();
        FinishGoal.SetActive(true);
        Barrier03.SetActive(true);
        Barrier04.SetActive(true);
        Barrier05.SetActive(true);
        //Barrier06.SetActive(true);
        //Barrier07.SetActive(true);

        int finishTFlag = 1;
        while (finishTFlag == 1)
        {
            if (movementType == 1)
            {
                finishTeleportCanvasText.text = "Great job!";
                instructions.Add("Time for a bigger challenge!");
                instructions.Add("Clear all of the hurdles along the path");
                finishTFlag = 0;
            }
            if (movementType == 0)
            {
                finishTeleportCanvasText.text = "Nice jump!";
                instructions.Add("Time to use your new teleport skills!");
                instructions.Add("Clear all of the hurdles along the path");
                finishTFlag = 0;
            }
            
            yield return null;
        }

        int finishTeleportFlag = 1;
        while (finishTeleportFlag == 1)
        {
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    finishTeleportCanvasText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton08.SetActive(false);
                FinishButton08.SetActive(true);
                last = 0;
            }

            if (tutorialButtons.finish == true)
            {
                FinishTeleportCanvas.SetActive(false);
                tutorialButtons.finish = false;
                finishTeleportFlag = 0;
            }

            yield return null;
        }

        //yield return StartCoroutine(PrintInstructions("finishTeleportCanvasText.text", index));
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        int endFlag = 1;
        while (endFlag == 1)
        {
            if (goalCount == 6 || goalCount == 4)
            {
                instructions.Clear();
                index = 0;
                endFlag = 0;
                StartCoroutine(FinishTutorial(hand, movementType));
            }
            yield return null;
        }
    }
    // --------------------------------------------------------------------

    /* Player reaches final goal */
    /* Teach player about kiosks */
    // --------------------------------------------------------------------
    IEnumerator FinishTutorial(int hand, int MovementType)
   {
        // Congrats!              You made it to the end!
        // If you would like to replay?
        // In the hub world there are glowing zones
        //these are called kiosks
        // kiosks are used to enter the minigames
        // Stand in a kiosks and press "A/X"
        // and the minigame will start shortly after
        //        // you have One more challenge!
        // Use the kiosk in front of you to journey to the hub world

        //yield return new WaitForSeconds(1);
        FinishCanvas.SetActive(true);
        //LocomotionSystem.GetComponent<TeleportationManager>().FreezePosition();

        finishCanvasText.text = "Congrats!\nYou made it to the end!";
        yield return new WaitForSeconds(3);

        int replayFlag = 1;
        while (replayFlag == 1)
        {
            if (tutorialButtons.next == true)
            {
                FinishCanvas.SetActive(false);
                ReplayCanvas.SetActive(true);
                replayCanvasText.text = "Would you like to replay?";

                int noReplayFlag = 1;
                while (noReplayFlag == 1)
                {
                    if (tutorialButtons.replayNo == true)
                    {
                        noReplayFlag = 0;
                        ReplayCanvas.SetActive(false);
                        FinishCanvas.SetActive(true);
                        NextButton09.SetActive(false);
                        FinishButton09.SetActive(true);
                    }
                    yield return null;
                }

                replayFlag = 0;
                tutorialButtons.next = false;
            }
            yield return null;
        }

        /*
        finishCanvasText.text = "In the hub world there are glowing zones";
        instructions.Add("these are called kiosks");
        instructions.Add("kiosks are used to enter the minigames");
        */
        /*
        int finishHand = 1;
        while (finishHand == 1)
        {
            if (hand == 0)
            {
                finishCanvasText.text = "Stand in the kiosks and press \"A\"";
                finishHand = 0;
            }
            if (hand == 1)
            {
                finishCanvasText.text = "Stand in the kiosks and press \"X\"";
                finishHand = 0;
            }
            
            yield return null;
        }*/

        finishCanvasText.text = "To leave stand in the circle and press \"X\" or \"A\"";

        //instructions.Add("and the minigame will start shortly after");
        //instructions.Add("Use the kiosk in front of you to journey to the hub world");

        int finishCanvasFlag = 1;
        while (finishCanvasFlag == 1)
        {   /*
            while (index < instructions.Count)
            {
                if (tutorialButtons.next == true)
                {
                    text = instructions[index];
                    finishCanvasText.text = text;
                    index++;
                    tutorialButtons.next = false;
                    yield return new WaitForSeconds(1);
                }
                last = 1;
                yield return null;
            }

            if (last == 1)
            {
                NextButton09.SetActive(false);
                FinishButton09.SetActive(true);
                last = 0;
            }
            */

            if (tutorialButtons.finish == true)
            {
                FinishCanvas.SetActive(false);
                tutorialButtons.finish = false;
                finishCanvasFlag = 0;
            }

            yield return null;
        }

        // yield return StartCoroutine(PrintInstructions("finishCanvasText.text", index));
        //LocomotionSystem.GetComponent<TeleportationManager>().UnfreezePosition();

        index = 0;
        instructions.Clear();
        //yield return new WaitForSeconds(5);
        //FinishCanvas.SetActive(false);
    }
   // --------------------------------------------------------------------
}