using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubworldMenu : MonoBehaviour
{
    public TMP_Text usernameText;
    public Image leftHandedButton, rightHandedButton, teleportButton, continuousButton, logoutPanel, tutorialButton;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    public GloveSkinner leftSkinner, rightSkinner;

    public SelectGlove sg;

    private GameObject locomotionSystem;

    void Start()
    {
        locomotionSystem = GameObject.Find("Locomotion System");

        usernameText.text = ELLEAPI.username;
        leftHandedButton.color = Color.white * (ELLEAPI.rightHanded ? .6f : .9f);
        rightHandedButton.color = Color.white * (ELLEAPI.rightHanded ? .9f : .6f);

        teleportButton.color = Color.white * (locomotionSystem.GetComponent<TeleportationManager>().teleportMovement ? .6f : .9f);
        continuousButton.color = Color.white * (locomotionSystem.GetComponent<TeleportationManager>().teleportMovement ? .9f : .6f);
    }

    void Update()
    {
        bool leftHandHovered, rightHandHovered, teleportHovered, continuousHovered, logoutHovered, tutorialHovered;
        bool leftHandSelected, rightHandSelected, teleportSelected, continuousSelected;
        RaycastHit hit;

        leftHandHovered = rightHandHovered = teleportHovered = continuousHovered = logoutHovered = tutorialHovered = false;
        leftHandSelected = !ELLEAPI.rightHanded;
        rightHandSelected = ELLEAPI.rightHanded;

        teleportSelected = locomotionSystem.GetComponent<TeleportationManager>().teleportMovement;
        continuousSelected = !teleportSelected;

        if (Physics.Raycast(leftHandPointer.position, leftHandPointer.forward, out hit) && leftHandSelected)
        {
            if (hit.transform.CompareTag("Menu"))
            {
                leftHandBeam.SetActive(true);
                leftHandDot.SetActive(true);
                leftHandDot.transform.eulerAngles = hit.normal;
                leftHandDot.transform.eulerAngles += new Vector3(0, 180, 0);
                leftHandDot.transform.position = hit.point + leftHandDot.transform.forward * -0.001f;

                if (hit.transform.name == "Left Hand Button")
                {
                    leftHandHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                        UpdateHandAndHopefullyInBackendToo(false);
                }
                if (hit.transform.name == "Right Hand Button")
                {
                    rightHandHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                        UpdateHandAndHopefullyInBackendToo(true);
                }
                if (hit.transform.name == "Teleport Button")
                {
                    teleportHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        PlayerPrefs.SetInt("teleportMovement", 1);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = true;
                        Debug.Log("Used left hand to select teleportation. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Continuous Button")
                {
                    continuousHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        PlayerPrefs.SetInt("teleportMovement", 0);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = false;
                        Debug.Log("Used left hand to select continuous. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Logout Panel")
                {
                    logoutHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                        Logout();
                }
                if (hit.transform.name == "Tutorial Button")
                {
                    tutorialHovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                        SceneManager.LoadScene("MovementTutorial");
                }

                if (VRInput.leftTriggerDigitalDown && hit.transform.name.Contains("Glove Button"))
                    sg.GloveSelected(int.Parse(hit.transform.name.Substring(13)));
                if (VRInput.leftTriggerDigitalDown && hit.transform.name == "Confirm Button")
                    sg.ConfirmSelection();
                if (VRInput.leftTriggerDigitalDown && hit.transform.name == "Cancel Button")
                    sg.CancelSelection();
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

                if (hit.transform.name == "Left Hand Button")
                {
                    leftHandHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                        UpdateHandAndHopefullyInBackendToo(false);
                }
                if (hit.transform.name == "Right Hand Button")
                {
                    rightHandHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                        UpdateHandAndHopefullyInBackendToo(true);
                }
                if (hit.transform.name == "Teleport Button")
                {
                    teleportHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        PlayerPrefs.SetInt("teleportMovement", 1);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = true;
                        Debug.Log("Used right hand to select teleportation. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Continuous Button")
                {
                    continuousHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        PlayerPrefs.SetInt("teleportMovement", 0);
                        locomotionSystem.GetComponent<TeleportationManager>().teleportMovement = false;
                        Debug.Log("Used right hand to select continuous. Now, teleportMovement = " + locomotionSystem.GetComponent<TeleportationManager>().teleportMovement);
                    }
                }
                if (hit.transform.name == "Logout Panel")
                {
                    logoutHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                        Logout();
                }
                if (hit.transform.name == "Tutorial Button")
                {
                    tutorialHovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                        SceneManager.LoadScene("MovementTutorial");
                }

                if (VRInput.rightTriggerDigitalDown && hit.transform.name.Contains("Glove Button"))
                        sg.GloveSelected(int.Parse(hit.transform.name.Substring(13)));
                if (VRInput.rightTriggerDigitalDown && hit.transform.name == "Confirm Button")
                    sg.ConfirmSelection();
                if (VRInput.rightTriggerDigitalDown && hit.transform.name == "Cancel Button")
                    sg.CancelSelection();
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

        if (leftHandHovered) 
            leftHandedButton.color = Color.white;
        else 
            leftHandedButton.color = Color.white * (leftHandSelected ? .9f : .6f);

        if (rightHandHovered) 
            rightHandedButton.color = Color.white;
        else 
            rightHandedButton.color = Color.white * (rightHandSelected ? .9f : .6f);


        if (teleportHovered)
            teleportButton.color = Color.white;
        else
            teleportButton.color = Color.white * (teleportSelected ? 0.95f : .5f);

        if (continuousHovered)
            continuousButton.color = Color.white;
        else
            continuousButton.color = Color.white * (continuousSelected ? 0.95f : .5f);
        
        if(tutorialHovered)
            tutorialButton.color = new Color(0.9960785f, 0.9607844f, 0.948f, 1f);
        else
            tutorialButton.color = new Color(0.9960785f, 0.9607844f, 0.7490196f, 1f);

        logoutPanel.color = new Color(1, 0.85f, 0.66f) * (logoutHovered ? .65f : 1);
    }

    void UpdateHandAndHopefullyInBackendToo(bool rightHanded)
    {
        ELLEAPI.rightHanded = rightHanded;
        ELLEAPI.UpdatePreferences();
    }

    void UpdateSkinAndHopefullyInBackendToo(string skin)
    {
        ELLEAPI.glovesSkin = skin;
        leftSkinner.UpdateSkin();
        rightSkinner.UpdateSkin();
        ELLEAPI.UpdatePreferences();
    }
    
    void Logout()
    {
        PlayerPrefs.DeleteKey("jwt");
        SceneManager.LoadScene("Login");
    }
}
