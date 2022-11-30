using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class CustomContinuousManager : ContinuousMoveProviderBase
    {
        [SerializeField] private XRRayInteractor _leftRayInteractor;
        [SerializeField] private XRRayInteractor _rightRayInteractor;

        private bool rightHandSelected, lastHandRight;
        public bool movementEnabled = true;
        
        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the left hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_LeftHandMoveAction;
        /// <summary>
        /// The Input System Action that will be used to read Move data from the left hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty leftHandMoveAction
        {
            get => m_LeftHandMoveAction;
            set => SetInputActionProperty(ref m_LeftHandMoveAction, value);
        }

        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the right hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_RightHandMoveAction;
        /// <summary>
        /// The Input System Action that will be used to read Move data from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty rightHandMoveAction
        {
            get => m_RightHandMoveAction;
            set => SetInputActionProperty(ref m_RightHandMoveAction, value);
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            rightHandSelected = ELLEAPI.rightHanded;
            lastHandRight = rightHandSelected;            
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            m_LeftHandMoveAction.DisableDirectAction();
            m_RightHandMoveAction.DisableDirectAction();
        }

        private void BindActions()
		{
            _leftRayInteractor.enabled = false;
            _rightRayInteractor.enabled = false;

            if (!rightHandSelected)
            {
                m_RightHandMoveAction.EnableDirectAction();
                m_LeftHandMoveAction.DisableDirectAction();
            }
            else
            {
                m_LeftHandMoveAction.EnableDirectAction();
                m_RightHandMoveAction.DisableDirectAction();
            }            
        }

        private void UpdateHandDominance()
        {
            rightHandSelected = ELLEAPI.rightHanded;

            if (lastHandRight && rightHandSelected)
            {
                lastHandRight = false;
                BindActions();
            }
            else if (!lastHandRight && !rightHandSelected)
            {
                lastHandRight = true;
                BindActions();
            }
        }

        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            movementEnabled = PlayerPrefs.GetInt("movementEnabled", 1) == 1;

            if (PlayerPrefs.GetInt("teleportMovement") == 1 || !movementEnabled)
            {
                GetComponent<CustomContinuousManager>().enabled = false;
            }

            UpdateHandDominance();

            if (!rightHandSelected)
			{
                var rightHandValue = m_RightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

                return rightHandValue;
            }
            else
			{
                var leftHandValue = m_LeftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

                return leftHandValue;
            }
        }

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }
    }
}

