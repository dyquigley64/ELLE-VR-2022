using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectTrigger : MonoBehaviour
{
    public GameObject levelSelect;

    private void OnTriggerEnter(Collider other)
    {
        if (levelSelect != null)
        {
            levelSelect.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (levelSelect != null)
        {
            levelSelect.SetActive(false);
        }
    }
}
