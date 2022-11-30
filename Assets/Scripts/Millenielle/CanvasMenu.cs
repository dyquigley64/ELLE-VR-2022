using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasMenu : MonoBehaviour
{

    [SerializeField] string buttonName;
    [SerializeField] Image button;
    bool hovered;
    public Fader blackFader;
    public Transform leftHandPointer, rightHandPointer;
    public GameObject leftHandBeam, rightHandBeam, leftHandDot, rightHandDot;
    
    // Start is called before the first frame update
    void Start()
    {
        //hovered = new bool[buttons.Length];
    }

    // Update is called once per frame
    void Update()
    {
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

                if (hit.transform.name == buttonName)
                {
                    hovered = true;
                    if (VRInput.leftTriggerDigitalDown)
                    {
                        Debug.Log("Used left hand to select Exit.");
                        StartCoroutine(GoToDestination("Hubworld"));
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

                if (hit.transform.name == buttonName)
                {
                    hovered = true;
                    if (VRInput.rightTriggerDigitalDown)
                    {
                        Debug.Log("Used right hand to select exit.");
                        StartCoroutine(GoToDestination("Hubworld"));
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
    public IEnumerator GoToDestination(string sceneName)
    {
        print("starting fade now");
        blackFader.Fade(true, 1f);
        print("starting wait now");
        yield return new WaitForSeconds(2);
        print("wait finished, now loading "+sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
