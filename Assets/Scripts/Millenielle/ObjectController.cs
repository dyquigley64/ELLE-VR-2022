using System.Collections;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public bool deposited, hintUsed;
    private Vector3 origPos;
    private Quaternion origRot;
    public string objectName, spanishName, objectHint;

    public HandController LeftHand, RightHand;

    private Transform blockParent;
    private Rigidbody r;
    private AudioSource a;

    private Vector3 lastPos;
    private float collisionTimer = 0.1f; // After starting game or resetting object, wait this many seconds before playing collision sounds
    public AudioClip[] collisionSounds;

    //public GameObject highlightedPulse;
    
    void Start()
    {
        if (gameObject.name.Length > 9)
            gameObject.name = gameObject.name.Substring(0, 9);
            
        blockParent = transform.parent;
        r = GetComponent<Rigidbody>();
        a = GetComponent<AudioSource>();

        deposited = hintUsed = false;
        origPos = transform.position;
        origRot = transform.rotation;

        if (string.IsNullOrEmpty(objectName)) objectName = "";
    }

    void Update()
    {
        if (collisionTimer > 0)
            collisionTimer -= Time.deltaTime;
    }

    // Resets the object's position to its initial position, and resets the deposited flag
    public void ResetPos()
    {
        collisionTimer = 0.1f;
        deposited = false;
        if (transform.parent == null || !transform.parent.CompareTag("Hand")) // Only reset if not currently being held
        {
            transform.position = origPos;
            transform.rotation = origRot;
            r.velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deposit"))
        {
            // Add the object to the player's collected items when they bring it to a level's deposit point
            Deposit(other);
        }
        else if (other.CompareTag("Reset"))
        {
            // Object went out of bounds, reset its position
            ResetPos();
        }
    }

    private void Deposit(Collider other)
    {
        if (deposited || other.GetComponent<DepositController>().objCount >= 5) return; // Do nothing if (somehow) already deposited or if deposit point is full

        LeftHand.DropCurrentBlock();
        RightHand.DropCurrentBlock();

        print("Entered");
        deposited = true;
        
        other.GetComponent<DepositController>().Pack(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionSounds.Length == 0) return;
        if (collisionTimer > 0) return; // Don't make a noise when first spawning in or resetting

        float soundIntensity = Mathf.InverseLerp(0, 1, (transform.position - lastPos).magnitude);

        float thingLeft = 1.0f;
        float currentThing;
        while (thingLeft > 0)
        {
            currentThing = Random.Range(0.0f, 1.0f);
            if (currentThing > thingLeft)
                currentThing = thingLeft + 0.000001f;
            a.PlayOneShot(collisionSounds[Random.Range(0, collisionSounds.Length)], currentThing * soundIntensity);
            thingLeft -= currentThing;
        }
    }
}