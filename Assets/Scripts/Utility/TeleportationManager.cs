using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;



public class TeleportationManager : MonoBehaviour

{
    [SerializeField] private InputActionReference leftActivate;
    [SerializeField] private XRRayInteractor _leftRayInteractor;
    [SerializeField] private XRInteractorLineVisual _leftRayReticle;
    [SerializeField] private InputActionReference rightActivate;
    [SerializeField] private XRRayInteractor _rightRayInteractor;
    [SerializeField] private XRInteractorLineVisual _rightRayReticle;
    [SerializeField] private XRRayInteractor _rightGrabRay;
    [SerializeField] private XRRayInteractor _leftGrabRay;
    [SerializeField] TeleportationProvider _teleportationProvider;
    [SerializeField] public bool teleportMovement;

    [SerializeField] private bool isMillenielle = false;
    [SerializeField] private Transform groundReference;

    public bool movementEnabled;
    private bool teleportWasDisabled;
    private bool leftHandSelected;
    private bool lastHandRight;

    void Start()
    {
        leftHandSelected = !ELLEAPI.rightHanded;
        lastHandRight = !leftHandSelected;

        teleportMovement = PlayerPrefs.GetInt("teleportMovement", 1) == 1;
        PlayerPrefs.SetInt("teleportMovement", teleportMovement ? 1 : 0);

        if (!teleportMovement)
            teleportWasDisabled = true;
        else
            teleportWasDisabled = false;

        movementEnabled = true;
        PlayerPrefs.SetInt("movementEnabled", movementEnabled ? 1 : 0);

        if (!teleportMovement)
            if (!movementEnabled)
                return;
            else
                GetComponent<CustomContinuousManager>().enabled = true;
        else
            GetComponent<CustomContinuousManager>().enabled = false;

        BindActions();
    }

    void Update()
    {
        teleportMovement = PlayerPrefs.GetInt("teleportMovement", 1) == 1;
        movementEnabled = PlayerPrefs.GetInt("movementEnabled", 1) == 1;
        
        if (!movementEnabled)
		{
            GetComponent<CustomContinuousManager>().enabled = false;
            UnbindActions();
            return;
        }
        else if (!teleportMovement)
		{
            teleportWasDisabled = true;
            GetComponent<CustomContinuousManager>().enabled = true;
            UnbindActions();
            return;
        }
        else
		{
            GetComponent<CustomContinuousManager>().enabled = false;
        }

        if (_leftRayInteractor == null || _rightRayInteractor == null)
            return;

        UpdateHandDominance();
    }

    public void BindActions()
	{
        if (!leftHandSelected)
        {
            leftActivate.action.performed += TeleportActivate;
            leftActivate.action.canceled += TeleportCanceled;

            rightActivate.action.performed -= TeleportActivate;
            rightActivate.action.canceled -= TeleportCanceled;
        }
        else
        {
            rightActivate.action.performed += TeleportActivate;
            rightActivate.action.canceled += TeleportCanceled;

            leftActivate.action.performed -= TeleportActivate;
            leftActivate.action.canceled -= TeleportCanceled;
        }

        _rightRayInteractor.enabled = false;
        _leftRayInteractor.enabled = false;
        _leftRayReticle.reticle.SetActive(false);
        _rightRayReticle.reticle.SetActive(false);
    }

    public void UnbindActions()
	{
        rightActivate.action.performed -= TeleportActivate;
        rightActivate.action.canceled -= TeleportCanceled;

        leftActivate.action.performed -= TeleportActivate;
        leftActivate.action.canceled -= TeleportCanceled;

        _rightRayInteractor.enabled = false;
        _leftRayInteractor.enabled = false;
        _leftRayReticle.reticle.SetActive(false);
        _rightRayReticle.reticle.SetActive(false);
    }

    private void UpdateHandDominance()
	{
        leftHandSelected = !ELLEAPI.rightHanded;

        if (lastHandRight && leftHandSelected)
        {
            lastHandRight = false;
            BindActions();
        }
        else if (!lastHandRight && !leftHandSelected)
		{
            lastHandRight = true;
            BindActions();
		}
        else if (teleportMovement && teleportWasDisabled)
		{
            teleportWasDisabled = false;
            BindActions();
        }
    }

    private void TeleportCanceled(InputAction.CallbackContext obj)
	{
        if (_leftRayInteractor == null || _rightRayInteractor == null)
            return;

		if (!leftHandSelected)
		{
            if (_leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit ray) && _leftRayInteractor.enabled == true)
            {
                TeleportRequest teleportRequest = new TeleportRequest();
                teleportRequest.destinationPosition = ray.point;
                if (!isMillenielle || ray.point.y <= groundReference.position.y + 0.1f) _teleportationProvider.QueueTeleportRequest(teleportRequest);
            }

            _leftGrabRay.enabled = true;
            _leftRayInteractor.enabled = false;
            _leftRayReticle.reticle.SetActive(false);
        }
        else
		{
            if (_rightRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit ray) && _rightRayInteractor.enabled == true)
            {
                TeleportRequest teleportRequest = new TeleportRequest();
                teleportRequest.destinationPosition = ray.point;
                if (!isMillenielle || ray.point.y <= groundReference.position.y + 0.1f) _teleportationProvider.QueueTeleportRequest(teleportRequest); // If in Milleni-ELLE, prevent teleporting through walls
            }

            _rightGrabRay.enabled = true;
            _rightRayInteractor.enabled = false;
            _rightRayReticle.reticle.SetActive(false);
        }
    }

    private void TeleportActivate(InputAction.CallbackContext obj)
    {
        UpdateHandDominance();

        if (!leftHandSelected)
        {
            if (_leftGrabRay != null)
                _leftGrabRay.enabled = false;

            if (_leftRayInteractor != null)
                _leftRayInteractor.enabled = true;

            if (_leftRayReticle != null || _leftRayReticle.reticle != null)
                _leftRayReticle.reticle.SetActive(true);
        }
        else
        {
            if (_rightGrabRay != null)
                _rightGrabRay.enabled = false;

            if (_rightRayInteractor != null)
                _rightRayInteractor.enabled = true;

            if (_rightRayReticle != null || _rightRayReticle.reticle != null)
                _rightRayReticle.reticle.SetActive(true);
        }
    }

    public void FreezePosition()
	{
        PlayerPrefs.SetInt("movementEnabled", 0);

        GetComponent<CustomActionBasedSnapTurnProvider>().enabled = false;
        GetComponent<CustomContinuousManager>().enabled = false;

        rightActivate.action.Disable();
        leftActivate.action.Disable();

        _rightRayInteractor.enabled = false;
        _leftRayInteractor.enabled = false;
        _leftRayReticle.reticle.SetActive(false);
        _rightRayReticle.reticle.SetActive(false);
    }

    public void UnfreezePosition()
	{
        teleportMovement = PlayerPrefs.GetInt("teleportMovement", 1) == 1;
        PlayerPrefs.SetInt("movementEnabled", 1);

        movementEnabled = true;
        GetComponent<CustomActionBasedSnapTurnProvider>().enabled = true;
        GetComponent<CustomContinuousManager>().enabled = true;

        rightActivate.action.Enable();
        leftActivate.action.Enable();

        if (teleportMovement)
            BindActions();
    }

    public void DisableRaycastGrab()
	{
        _rightGrabRay.enabled = false;
        _leftGrabRay.enabled = false;
    }

    public void EnableRaycastGrab()
    {
        _rightGrabRay.enabled = true;
        _leftGrabRay.enabled = true;
    }
}
