using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefCounter : MonoBehaviour
{
    public bool roundFinished;

    // Start is called before the first frame update
    void Start()
    {
        roundFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finished Plate"))
        {
            roundFinished = true;
        }
    }
}
