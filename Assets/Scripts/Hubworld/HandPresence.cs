using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public GameObject handModelPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;

    private InputDevice targetDevice;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    private List<InputDevice> devices;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
	{
        devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
		{
            targetDevice = devices[0];
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
		}
	}

    void UpdateHandAnimation()
	{
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
		{
            handAnimator.SetFloat("Trigger", triggerValue);
		}
        else
        {
            handAnimator.SetFloat("Trigger", 0);
		}

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (devices.Capacity < 2)
		{
            TryInitialize();
		}
        else
		{
            spawnedHandModel.SetActive(true);
            UpdateHandAnimation();
        }
    }
}
