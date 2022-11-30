using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Stop playing the hint audio and timer when all the ingredients have been thrown into the pot
public class Hint : MonoBehaviour
{
    public CheckFoodAnswer checkFoodAnswer;
    public AudioClip hintAudio;

    public bool isTutorial = false;
    private bool tutorialAudioPlaying = false;

    private AudioSource sfx;


    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        sfx.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkFoodAnswer == null)
            return;

        if (isTutorial && !tutorialAudioPlaying)
		{
            tutorialAudioPlaying = true;
            sfx.loop = true;
            sfx.Play();
        }

        List<string> correctTerms = checkFoodAnswer.correctTerms;
        
        if (CheckFoodAnswer.resetHintTimer && checkFoodAnswer.correctTerms.Contains(gameObject.transform.parent.name))
        {
            StopAllCoroutines(); // used to "reset" the timer
            CheckFoodAnswer.resetHintTimer = false;
            StartCoroutine(Timer());
            sfx.loop = false;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(30f); // wait 30 seconds to play hint audio

        if (!CheckFoodAnswer.resetHintTimer)
        {
            sfx.loop = true;
            sfx.Play();
        }
    }

    void OnDisable()
    {
        sfx = GetComponent<AudioSource>();
        sfx.loop = false;
    }
    void OnEnable()
    {
        sfx = GetComponent<AudioSource>();
        sfx.loop = false;
    }
}
