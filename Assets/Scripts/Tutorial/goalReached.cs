using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalReached : MonoBehaviour
{
    public GameObject goal;
    public TutorialManager tutorialManager;
    private AudioSource audio;
    public AudioClip reached;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(reached, transform.position, 0.6f);
        tutorialManager.goalCount++;
        goal.SetActive(false);
    }
}