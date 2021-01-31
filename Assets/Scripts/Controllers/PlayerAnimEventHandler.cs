using UnityEngine;

namespace GGJ2021
{
    public class PlayerAnimEventHandler : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayStepSound()
        {
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("player_footsteps");
        }

        public void PlayJumpSound()
        {

        }

        public void PlayPlayerDamageSound()
        {

        }

        public void PlayAcidDamagedSound()
        {

        }

        public void PlayPlayerDeathSound()
        {

        }
    }
}