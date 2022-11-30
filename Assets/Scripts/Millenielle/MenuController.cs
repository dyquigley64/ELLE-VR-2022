using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image resetButton, unpackButton; // Teleport -> Reset, Continuous -> Unpack
    public Image[] unpackButtons;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;

    public GameObject[] Objects;
    public DepositController Deposit;

    private AudioSource audio;
    public AudioClip resetSound, unpackSound;

    void Start()
    {
        resetButton.color = Color.white * .6f;
        //unpackButton.color = Color.white * .6f;
        foreach (Image img in unpackButtons)
        {
            img.color = Color.white * .6f;
        }
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool resetHovered, unpack1Hovered, unpack2Hovered, unpack3Hovered, unpack4Hovered, unpack5Hovered;
        bool leftHandSelected, rightHandSelected;
        RaycastHit hit;

        resetHovered = unpack1Hovered = unpack2Hovered = unpack3Hovered = unpack4Hovered = unpack5Hovered = false;
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

                if (hit.transform.name == "Reset Button")
                {
                    resetHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select reset.");
                        Reset();
                    }
                }
                /*
                if (hit.transform.name == "Unpack Button")
                {
                    unpackHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack.");
                        Unpack();
                    }
                }
                */
                if (hit.transform.name == "Item 1 Unpack Button")
                {
                    unpack1Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack 1.");
                        Unpack(0);
                    }
                }
                if (hit.transform.name == "Item 2 Unpack Button")
                {
                    unpack2Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack 2.");
                        Unpack(1);
                    }
                }
                if (hit.transform.name == "Item 3 Unpack Button")
                {
                    unpack3Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack 3.");
                        Unpack(2);
                    }
                }
                if (hit.transform.name == "Item 4 Unpack Button")
                {
                    unpack4Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack 4.");
                        Unpack(3);
                    }
                }
                if (hit.transform.name == "Item 5 Unpack Button")
                {
                    unpack5Hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select unpack 5.");
                        Unpack(4);
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

                if (hit.transform.name == "Reset Button")
                {
                    resetHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select reset.");
                        Reset();
                    }
                }
                /*
                if (hit.transform.name == "Unpack Button")
                {
                    unpackHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack.");
                        Unpack();
                    }
                }
                */
                if (hit.transform.name == "Item 1 Unpack Button")
                {
                    unpack1Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack 1.");
                        Unpack(0);
                    }
                }
                if (hit.transform.name == "Item 2 Unpack Button")
                {
                    unpack2Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack 2.");
                        Unpack(1);
                    }
                }
                if (hit.transform.name == "Item 3 Unpack Button")
                {
                    unpack3Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack 3.");
                        Unpack(2);
                    }
                }
                if (hit.transform.name == "Item 4 Unpack Button")
                {
                    unpack4Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack 4.");
                        Unpack(3);
                    }
                }
                if (hit.transform.name == "Item 5 Unpack Button")
                {
                    unpack5Hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select unpack 5.");
                        Unpack(4);
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

        if (resetHovered)
            resetButton.color = Color.white;
        else
            resetButton.color = Color.white * 0.8f;

        /*
        if (unpackHovered)
            unpackButton.color = Color.white;
        else
            unpackButton.color = Color.white * 0.8f;
        */

        if (unpack1Hovered)
            unpackButtons[0].color = Color.white;
        else
            unpackButtons[0].color = Color.white * 0.8f;

        if (unpack2Hovered)
            unpackButtons[1].color = Color.white;
        else
            unpackButtons[1].color = Color.white * 0.8f;

        if (unpack3Hovered)
            unpackButtons[2].color = Color.white;
        else
            unpackButtons[2].color = Color.white * 0.8f;

        if (unpack4Hovered)
            unpackButtons[3].color = Color.white;
        else
            unpackButtons[3].color = Color.white * 0.8f;

        if (unpack5Hovered)
            unpackButtons[4].color = Color.white;
        else
            unpackButtons[4].color = Color.white * 0.8f;
    }

    // Resets the position of all non-deposited objects
    void Reset()
    {
        if (audio != null) audio.PlayOneShot(resetSound, 0.7f);

        foreach (GameObject i in Objects)
        {
            ObjectController obj = i.GetComponent<ObjectController>();
            if (!obj.deposited)
                obj.ResetPos();
        }
    }

    /*// Unpacks and resets the position of the most recently deposited object
    void Unpack()
    {
        Deposit.Unpack();
    }
    */

    // Unpacks and resets the position of a certain deposited object
    void Unpack(int id)
    {
        if (audio != null) audio.PlayOneShot(unpackSound, 0.7f);

        Deposit.Unpack(id);
    }
}
