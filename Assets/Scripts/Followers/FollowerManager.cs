namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// Tracks the ability hamsters following the player.
    /// </summary>
    class FollowerManager : MonoBehaviour {

        public static FollowerManager instance;

        [SerializeField]
        private GameObject followerGrapple;
        [SerializeField]
        private GameObject followerGrenade;
        [SerializeField]
        private GameObject followerDash;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            foreach (Ability ability in new Ability[] { Ability.Grapple, Ability.Grenade, Ability.Dash })
            {
                if (PlayerStats.instance.HasAbility(ability))
                {
                    AddAbilityHamster(ability);
                }
            }
        }

        public void AddAbilityHamster(Ability ability)
        {
            GameObject hamster = null;
            switch(ability)
            {
                case Ability.Grapple:
                    hamster = followerGrapple;
                    break;
                case Ability.Grenade:
                    hamster = followerGrenade;
                    break;
                case Ability.Dash:
                    hamster = followerDash;
                    break;
            }
            hamster.SetActive(true);
            hamster.transform.position = PlayerController.instance.transform.position;
        }
    }
}