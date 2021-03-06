namespace GGJ2021
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Keeps track of player attributes that persist between stages, like collectibles and health.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats instance;

        [Tooltip("Starting max health without upgrades.")]
        [SerializeField]
        private int startingMaxHealth = 3;

        /// <summary>
        /// Max health including upgrades.
        /// </summary>
        private int maxHealth;
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        /// <summary>
        /// Current player health.
        /// </summary>
        private int curHealth;
        public int CurHealth
        {
            get { return curHealth; }
            set { curHealth = Math.Min(value, maxHealth); }
        }

        /// <summary>
        /// Tracks which collectibles have been collected, split by collectible type.
        /// </summary>
        private Dictionary<Type, HashSet<string>> collectedCollectibles = new Dictionary<Type, HashSet<string>>();

        [Tooltip("Treat all abilities as unlocked, even without the required hamsters.")]
        [SerializeField]
        private bool unlockAllAbilities;

        public int spawnDoor = -1;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (MaxHealth <= 0)
            {
                MaxHealth = startingMaxHealth;
            }
            if (CurHealth <= 0)
            {
                CurHealth = MaxHealth;
            }
        }

        /// <summary>
        /// Checks if a collectible is already collected.
        /// </summary>
        public bool IsCollectibleCollected(Collectible collectible)
        {
            Type collectibleType = collectible.GetType();
            if (collectedCollectibles.ContainsKey(collectibleType))
            {
                return collectedCollectibles[collectibleType].Contains(collectible.Id);
            }
            return false;
        }

        /// <summary>
        /// Registers a collectible as collected.
        /// </summary>
        public void CollectCollectible(Collectible collectible)
        {
            Type collectibleType = collectible.GetType();
            if (!collectedCollectibles.ContainsKey(collectibleType))
            {
                collectedCollectibles[collectibleType] = new HashSet<string>();
            }
            collectedCollectibles[collectibleType].Add(collectible.Id);
        }

        public bool HasAbility(Ability ability)
        {
            if (unlockAllAbilities)
            {
                return true;
            }
            Type collectibleType = typeof(AbilityHamster);
            if (collectedCollectibles.ContainsKey(collectibleType))
            {
                return collectedCollectibles[collectibleType].Contains(ability.ToString());
            }
            return false;
        }

        public void Reset()
        {
            maxHealth = startingMaxHealth;
            curHealth = maxHealth;
            collectedCollectibles.Clear();
            spawnDoor = -1;
        }
    }
}
