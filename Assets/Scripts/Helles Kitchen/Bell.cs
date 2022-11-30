using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public GameObject chefAudio;

    public bool tutorialBell = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        chefAudio.GetComponent<ChefAudio>().playAgain = true;
        chefAudio.GetComponent<ChefAudio>().PlayQuestion();

        tutorialBell = false;
    }
}