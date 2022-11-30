using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionsController : MonoBehaviour
{
    public ExitMenuController ExitMenu;
    private GameObject[] ObjectsNeeded;

    public TextMeshProUGUI ObjectList;
    private string str;

    // Start is called before the first frame update
    void Start()
    {
        if (ExitMenu != null) StartCoroutine(WaitThenStart(0.1f)); // If  ExitMenu is null, then this isn't a level with instructions
    }

    // Wait before setting instructions text to ensure the ObjectsNeeded list is finished being initialized
    IEnumerator WaitThenStart(float t)
    {
        print("now waiting for " + t + "seconds");
        yield return new WaitForSeconds(t);
        print("finished waiting for " + t + "seconds");
        
        ObjectsNeeded = ExitMenu.ObjectsNeeded;
        str = "";

        foreach (GameObject i in ObjectsNeeded)
        {
            string objName = i.GetComponent<ObjectController>().spanishName;

            if (string.IsNullOrEmpty(objName)) objName = "OBJECT NOT NAMED"; // Check if object being added has a name assigned

            str += objName + "\n";
        }
        print("final object list is:\n" + str);
        ObjectList.SetText(str);
    }
}
