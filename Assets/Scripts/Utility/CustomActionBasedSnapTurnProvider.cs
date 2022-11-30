using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// A locomotion provider that allows the user to rotate their rig using a 2D axis input
    /// from an input system action.
    /// </summary>
    [AddComponentMenu("XR/Locomotion/Snap Turn Provider (Action-based)")]
    public class CustomActionBasedSnapTurnProvider : SnapTurnProviderBase
    {
        private bool rightHandSelected, lastHandRight;

        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Snap Turn data from the left hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_LeftHandSnapTurnAction;
        /// <summary>
        /// The Input System Action that will be used to read Snap Turn data sent from the left hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty leftHandSnapTurnAction
        {
            get => m_LeftHandSnapTurnAction;
            set => SetInputActionProperty(ref m_LeftHandSnapTurnAction, value);
        }

        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Snap Turn data from the right hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_RightHandSnapTurnAction;
        /// <summary>
        /// The Input System Action that will be used to read Snap Turn data sent from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty rightHandSnapTurnAction
        {
            get => m_RightHandSnapTurnAction;
            set => SetInputActionProperty(ref m_RightHandSnapTurnAction, value);
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
            m_LeftHandSnapTurnAction.DisableDirectAction();
            m_RightHandSnapTurnAction.DisableDirectAction();
        }

        private void BindActions()
        {
            if (rightHandSelected)
            {
                m_RightHandSnapTurnAction.EnableDirectAction();
                m_LeftHandSnapTurnAction.DisableDirectAction();
            }
            else
            {
                m_LeftHandSnapTurnAction.EnableDirectAction();
                m_RightHandSnapTurnAction.DisableDirectAction();
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
            UpdateHandDominance();

            if (rightHandSelected)
			{
                var rightHandValue = m_RightHandSnapTurnAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
                return rightHandValue;
            }
            else
			{
                var leftHandValue = m_LeftHandSnapTurnAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
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
