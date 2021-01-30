namespace GGJ2021
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerAnimationController : MonoBehaviour
    {
        public Animator playerAnim;
        public List<SpriteRenderer> sprites;

        private string deathTrigger = "Death";
        private string jumpTrigger = "Jump";
        private string hurtTrigger = "Hurt";
        private string hurtAcidTrigger = "HurtAcid";
        private string walkingBool = "Walking";
        private string inAirBool = "InAir";

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

        public void EnableSprites()
        {
            sprites.ForEach(p => p.enabled = true);
        }

        public void DisableSprites()
        {
            sprites.ForEach(p => p.enabled = false);
        }
    }
}
