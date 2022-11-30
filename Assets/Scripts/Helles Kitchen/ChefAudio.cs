using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefAudio : MonoBehaviour
{
    public HellesKitchenManager hellesKitchenManager;
    public AudioSource questionClip;

    public bool playAgain;
    // Start is called before the first frame update
    void Start()
    {
        playAgain = true;
        //questionClip = hellesKitchenManager.ChefAudio.GetComponent<AudioSource>();
        /*questionClip.playOnAwake = false;*/

        
    }

    public void PlayQuestion()
    {
        if (questionClip.clip == null) 
            return;

        questionClip.loop = false;
        if (!questionClip.isPlaying && playAgain == true)
        {
            questionClip.Play();
            playAgain = false;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayQuestion();
           
        
        
        
        //sfx.Play();
        // will play question once. Have to see if when its a new round whether itll automatically update to the newest clip or to wait for it to update
        //PlayQuestion(hellesKitchenManager.ChefAudio.gameObject);
    }
}
