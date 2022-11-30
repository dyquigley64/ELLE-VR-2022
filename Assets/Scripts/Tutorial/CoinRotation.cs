using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public int rotateSpeed = 1;
    public GameObject coin;
    private AudioSource audio;
    public AudioClip coinReached;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.timeScale, 0, Space.World);
    }

    public void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(coinReached, transform.position, 0.6f);
        coin.SetActive(false);
    }
}
