using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject infoDialogueCanvas;
    public GameObject ADialogueCanvas;
    public GameObject HMDialogueCanvas;
    public GameObject BackpackInfoCanvas;

    public GameObject ArrowA;
    public GameObject ArrowHM;

    private void OnTriggerEnter(Collider other)
    {
        if (infoDialogueCanvas != null)
        {
            infoDialogueCanvas.SetActive(true);
        }
        else if (ADialogueCanvas != null)
        {
            ADialogueCanvas.SetActive(true);
            ArrowA.SetActive(false);
        }
        else if (HMDialogueCanvas != null)
        {
            HMDialogueCanvas.SetActive(true);
            ArrowHM.SetActive(false);
        }
        else if (BackpackInfoCanvas != null)
        {
            BackpackInfoCanvas.SetActive(true);
            ArrowHM.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (infoDialogueCanvas != null)
        {
            infoDialogueCanvas.SetActive(false);
            ArrowA.SetActive(true);
        }
        else if (ADialogueCanvas != null)
        {
            ADialogueCanvas.SetActive(false);
        }
        else if (HMDialogueCanvas != null)
        {
            HMDialogueCanvas.SetActive(false);
        }
        else if (BackpackInfoCanvas != null)
        {
            BackpackInfoCanvas.SetActive(false);
        }
    }
}
