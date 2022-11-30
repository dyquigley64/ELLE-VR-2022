using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTutorialBlock : MonoBehaviour
{
    public bool grabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Tutorial Table - Top"))
        {
            grabbed = true;

            transform.GetChild(0).GetComponent<Hint>().isTutorial = false;
            transform.GetChild(0).GetComponent<AudioSource>().Stop();
        }

        
    }
}
