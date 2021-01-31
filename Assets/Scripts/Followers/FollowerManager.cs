namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Tracks the ability hamsters following the player.
    /// </summary>
    class FollowerManager : MonoBehaviour {

        public static FollowerManager instance;

        [SerializeField]
        private FollowerPhysics followerGrapple;
        [SerializeField]
        private FollowerPhysics followerGrenade;
        [SerializeField]
        private FollowerPhysics followerDash;

        private List<FollowerPhysics> activeFollowers = new List<FollowerPhysics>();

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
            FollowerPhysics hamster = null;
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

            if (!hamster.gameObject.activeSelf)
            {
                hamster.gameObject.SetActive(true);
                GameObject sourceObject;
                if (activeFollowers.Count == 0)
                {
                    sourceObject = PlayerController.instance.gameObject;
                    // Offset the first hamster to account for the player's height.
                    hamster.config.targetOffset.y = -1f;
                }
                else
                {
                    sourceObject = activeFollowers[activeFollowers.Count - 1].gameObject;
                }
                hamster.sourceObj = sourceObject;
                hamster.transform.position = PlayerController.instance.transform.position;
                activeFollowers.Add(hamster);
            }
        }
    }
}