namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// An area is a teleportation destination which teleports the user to their pointed
    /// location on a surface.
    /// </summary>
    /// <seealso cref="TeleportationAnchor"/>
    //[HelpURL(XRHelpURLConstants.k_TeleportationArea)] // This line gives an error if not commented
    public class NotTeleportationArea : BaseTeleportationInteractable
    {
        /// <inheritdoc />
        protected override bool GenerateTeleportRequest(IXRInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
        {
            return false;
        }
    }
}
