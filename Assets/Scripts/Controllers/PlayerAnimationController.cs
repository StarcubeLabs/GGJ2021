namespace GGJ2021
{
    using UnityEngine;

    public class PlayerAnimationController : MonoBehaviour
    {
        public Animator playerAnim;
        public Animator cannonAnim;

        private string deathTrigger = "Death";
        private string jumpTrigger = "Jump";
        private string hurtTrigger = "Hurt";
        private string hurtAcidTrigger = "HurtAcid";
        private string walkingBool = "Walking";
        private string inAirBool = "InAir";

        private string fireTrigger = "Fire";
        private string grappleTrigger = "Grapple";

        public void SetIsWalk(bool isWalk)
        {
            playerAnim.SetBool(walkingBool, isWalk);
        }

        public void SetInAir(bool inAir)
        {
            playerAnim.SetBool(inAirBool, inAir);
        }

        public void DeathTrigger()
        {
            playerAnim.SetTrigger(deathTrigger);
            SetIsWalk(false);
            SetInAir(false);
        }

        public void JumpTrigger()
        {
            playerAnim.SetTrigger(jumpTrigger);
        }

        public void HurtTrigger()
        {
            playerAnim.SetTrigger(hurtTrigger);
        }

        public void HurtAcidTrigger()
        {
            playerAnim.SetTrigger(hurtAcidTrigger);
        }

        public void FireCannon()
        {
            cannonAnim.SetTrigger(fireTrigger);
        }

        public void FireGrapple()
        {
            cannonAnim.SetTrigger(grappleTrigger);
        }
    }
}
