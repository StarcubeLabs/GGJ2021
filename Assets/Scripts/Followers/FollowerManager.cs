namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Tracks the ability hamsters following the player.
    /// </summary>
    class FollowerManager : MonoBehaviour {

        public static FollowerManager instance;

        private static float FIRST_HAMSTER_OFFSET = -1f;

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
            FollowerPhysics hamster = GetHamster(ability);

            if (!hamster.gameObject.activeSelf)
            {
                hamster.gameObject.SetActive(true);
                hamster.enabled = true;
                hamster.transform.position = PlayerController.instance.transform.position;
                activeFollowers.Add(hamster);
                ConfigureActiveFollowers();
            }
        }

        public void RemoveAbilityHamster(Ability ability)
        {
            FollowerPhysics removeHamster = GetHamster(ability);
            if (activeFollowers.Contains(removeHamster))
            {
                removeHamster.gameObject.SetActive(false);
                removeHamster.enabled = false;
                List<FollowerPhysics> newFollowers = new List<FollowerPhysics>();
                foreach (FollowerPhysics hamster in activeFollowers)
                {
                    if (hamster != removeHamster)
                    {
                        newFollowers.Add(hamster);
                    }
                }
                activeFollowers = newFollowers;
                ConfigureActiveFollowers();
            }
        }

        private void ConfigureActiveFollowers()
        {
            for (int i = 0; i < activeFollowers.Count; i++)
            {
                FollowerPhysics hamster = activeFollowers[i];
                GameObject sourceObject;
                if (i == 0)
                {
                    sourceObject = PlayerController.instance.gameObject;
                    // Offset the first hamster to account for the player's height.
                    hamster.config.targetOffset.y = FIRST_HAMSTER_OFFSET;
                }
                else
                {
                    sourceObject = activeFollowers[i - 1].gameObject;
                    hamster.config.targetOffset.y = 0;
                }
                hamster.sourceObj = sourceObject;
            }
        }

        private FollowerPhysics GetHamster(Ability ability)
        {
            switch (ability)
            {
                case Ability.Grapple: return followerGrapple;
                case Ability.Grenade: return followerGrenade;
                case Ability.Dash: return followerDash;
            }
            return followerGrapple;
        }
    }
}