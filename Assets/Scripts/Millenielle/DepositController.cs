using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepositController : MonoBehaviour
{
    //public Stack<GameObject> DepositedObjects;
    public GameObject[] ItemNames, UnpackButtons, UnpackTexts, DepositedObjects;
    public int objCount = 0;

    public bool isTutorial = false;
    public DMHostMom momDialogueManager;

    private AudioSource audio;
    public AudioClip depositSound;

    // Start is called before the first frame update
    void Start()
    {
        if (isTutorial)
            return;
        
        //DepositedObjects = new Stack<GameObject>();
        DepositedObjects = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            DepositedObjects[i] = null;
            UnpackButtons[i].SetActive(false);
            UnpackTexts[i].SetActive(false);
            ItemNames[i].SetActive(false);
        }

        audio = GetComponent<AudioSource>();
    }

    // Adds a particular object to the DepositedObjects list and disables it, then enables the label for it on the unpack menu
    public void Pack(GameObject obj)
    {
        if (isTutorial) // Don't need to do much in the tutorial
        {
            obj.SetActive(false);
            if (audio != null) audio.PlayOneShot(depositSound, 0.7f);
            momDialogueManager.DisplayFinalSentence();
            return;
        }

        int index = -1;
        for (int i = 0; i < 5; i++) // Get first open index
        {
            if (DepositedObjects[i] == null)
            {
                index = i;
                break;
            }
        }
        if (index == -1) // Array is full, don't deposit
            return;

        //DepositedObjects.Push(obj);
        DepositedObjects[index] = obj;
        obj.SetActive(false);
        objCount++;
        if (audio != null) audio.PlayOneShot(depositSound, 0.7f);

        ItemNames[index].SetActive(true);
        ItemNames[index].GetComponent<TextMeshProUGUI>().SetText(obj.GetComponent<ObjectController>().objectName); // Update label for object name
        UnpackButtons[index].SetActive(true);
        UnpackTexts[index].SetActive(true);
    }

    /*
    // Removes the last deposited object from the DepositedObjects list and resets its position
    public void Unpack()
    {
        if (DepositedObjects.Count == 0) return; // Do nothing if empty

        GameObject obj = DepositedObjects.Pop();
        obj.SetActive(true);
        obj.GetComponent<ObjectController>().ResetPos();
    }
    */

    // Unpack the object at a certain index and disable its label on the unpack menu
    public void Unpack(int index)
    {
        if (DepositedObjects[index] == null) // Null check, just in case
            return;

        DepositedObjects[index].SetActive(true);
        DepositedObjects[index].GetComponent<ObjectController>().ResetPos();
        DepositedObjects[index] = null;
        objCount--;

        ItemNames[index].SetActive(false);
        UnpackButtons[index].SetActive(false);
        UnpackTexts[index].SetActive(false);
    }
}
