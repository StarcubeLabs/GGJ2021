namespace GGJ2021
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Generates unique IDs for collectibles in a tilemap.
    /// </summary>
    class CollectibleIndexer : MonoBehaviour
    {

        private Dictionary<Type, int> collectibleIndices = new Dictionary<Type, int>();

        public string GenerateId(Collectible collectible)
        {
            Type collectibleType = collectible.GetType();
            if (!collectibleIndices.ContainsKey(collectibleType))
            {
                collectibleIndices[collectibleType] = 0;
            }
            int collectibleIndex = collectibleIndices[collectibleType]++;
            return SceneManager.GetActiveScene().name + collectibleIndex;
        }
    }
}