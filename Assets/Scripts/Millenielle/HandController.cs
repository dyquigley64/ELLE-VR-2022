using UnityEngine;

public class HandController : MonoBehaviour
{
    private bool inGame = false;
    public GameObject pointer;
    public GameObject missBeam, hitBeam, activeBeam, hitMarker;
    public GameObject Phone;

    public bool isRightHand;
    //private Vector2 axis;
    private bool gripping, gripDown, released;


    public float pushPullFactor = 2.5f;
    private Vector3 lastPos, currentPos;
    public float rotationSpeed = 10;
    public Transform blockRotationTarget;

    public Transform currentBlock;
    public Transform blockParent;

    public LayerMask lm;

    private void Start()
    {
        if (ELLEAPI.rightHanded == isRightHand) // Dominant hand is the one used to hold objects
        {
            StartGame();
        }
        else // Non-dominant hand is used to hold phone for instructions and moving
        {
            Phone.SetActive(true);
            if (isRightHand)
                GameObject.Find("[Right Hand] Model Parent").SetActive(false);
            else
                GameObject.Find("[Left Hand] Model Parent").SetActive(false);
        }
    }

    public void StartGame()
    {
        inGame = true;
    }

    public void EndGame()
    {
        hitBeam.SetActive(false);
        missBeam.SetActive(false);
        hitMarker.SetActive(false);
        inGame = false;
    }

    private void Update()
    {
        if (inGame)
            InGameRoutine();
    }

    private void InGameRoutine()
    {
        //axis = !isRightHand ? VRInput.leftStick : VRInput.rightStick;
        gripping = !isRightHand ? VRInput.leftGripDigital : VRInput.rightGripDigital;
        gripDown = !isRightHand ? VRInput.leftGripDown : VRInput.rightGripDown;

        if (!released && currentBlock != null && !gripping) // If holding a object, keep track of when player stops holding grip
        {
            //DropCurrentBlock();
            released = true;
        }

        if (released && currentBlock != null && gripDown) // Grip was released, so on next grip pull drop object
        {
            DropCurrentBlock();
        }

        else if (Physics.Raycast(pointer.transform.position, pointer.transform.forward, out RaycastHit hit, 10, lm))
        {
            if (hit.transform.CompareTag("Block"))
            {
                hitBeam.SetActive(currentBlock == null);
                missBeam.SetActive(false);
                if (gripDown && currentBlock != hit.transform && (hit.transform.parent == null || !hit.transform.parent.name.Contains("Hand")))
                {
                    ChangeCurrentBlock(hit.transform);
                    released = false;
                }
            }
            else
            {
                hitBeam.SetActive(false);
                missBeam.SetActive(currentBlock == null);
            }
            hitMarker.SetActive(true);
            hitMarker.transform.position = hit.point;
            hitMarker.transform.rotation = Quaternion.LookRotation(-hit.normal);
            hitMarker.transform.position -= hitMarker.transform.forward * 0.001f; // To avoid z-fighting
        }
        else
        {
            hitBeam.SetActive(false);
            missBeam.SetActive(currentBlock == null);
            hitMarker.SetActive(false);
        }

        if (currentBlock != null)
        {
            // Rotate block
            currentPos = transform.position;
            //blockRotationTarget.rotation *= Quaternion.AngleAxis(-10 * axis.x * rotationSpeed * Time.deltaTime, Vector3.up);
            //blockRotationTarget.rotation *= Quaternion.AngleAxis(-10 * axis.y * rotationSpeed * Time.deltaTime, transform.right);
            //currentBlock.rotation = Quaternion.Lerp(currentBlock.rotation, blockRotationTarget.rotation, 10 * Time.deltaTime);

            // Take the position delta
            Vector3 posDelta = lastPos - currentPos;

            // Take the part of it that is push pull
            float dot = Vector3.Dot(transform.forward, posDelta);

            // Push/pull the block based on that
            currentBlock.localPosition = new Vector3(currentBlock.localPosition.x, currentBlock.localPosition.y, currentBlock.localPosition.z - dot * pushPullFactor);

            lastPos = transform.position;
        }
    }

    public void DropCurrentBlock()
    {
        if (currentBlock == null) return;

        currentBlock.GetComponent<Rigidbody>().isKinematic = false;
        currentBlock.GetComponent<PhysicsThrow>().ActivatePhysicsThrow();
        currentBlock.parent = blockParent;
        currentBlock = null;

        activeBeam.SetActive(false);

        if (!isRightHand) VRInput.LeftHandContinuousVibration(false, 0);
        else              VRInput.RightHandContinuousVibration(false, 0);
    }

    void ChangeCurrentBlock(Transform newBlock)
    {
        currentBlock = newBlock;

        lastPos = currentPos = transform.position;
        currentBlock.GetComponent<Rigidbody>().isKinematic = true;
        currentBlock.parent = transform;
        currentBlock.GetComponent<PhysicsThrow>().StartPhysicsThrowCapture();
        blockRotationTarget.rotation = Quaternion.identity;

        activeBeam.SetActive(true);

        if (!isRightHand) VRInput.LeftHandContinuousVibration(true, 0.1f);
        else              VRInput.RightHandContinuousVibration(true, 0.1f);
    }
}
