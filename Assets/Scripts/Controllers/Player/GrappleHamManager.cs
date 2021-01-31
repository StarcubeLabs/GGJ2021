namespace GGJ2021
{
    using UnityEngine;

    public class GrappleHamManager : MonoBehaviour
    {
        public SpriteRenderer ham;
        private bool grappleVisible;
        private bool grapplegrabbed;

        // Update is called once per frame
        void Update()
        {
            grapplegrabbed = PlayerController.instance.playerPhysics.isGrappling;
            grappleVisible = PlayerController.instance.playerGrappleManager.GrappleActive() || grapplegrabbed;

            ham.transform.position = PlayerController.instance.playerGrappleManager.GetGrappleEndPos();



            ham.enabled = grappleVisible;
        }

        private void LateUpdate()
        {
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * -PlayerController.instance.playerGrappleManager.GetGrappleDirection();
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            ham.transform.rotation = targetRotation;
        }
    }
}