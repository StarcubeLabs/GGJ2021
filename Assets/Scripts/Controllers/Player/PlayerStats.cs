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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        private void Start()
        {
            MaxHealth = startingMaxHealth;
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
    }
}
