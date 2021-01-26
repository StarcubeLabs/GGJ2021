namespace GGJ2021.Util
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationStateMapper<T> where T : struct, IConvertible
    {
        /// <summary> Mapping between state hashes and state names. </summary>
        private Dictionary<int, T> stateMap;
        /// <summary> The animator being tracked. </summary>
        private Animator anim;
        /// <summary> The layer being tracked. </summary>
        private int layer;

        /// <summary> Creates a Mapping between Animator state hashes and the AnimatorState enum. </summary>
        /// <param name="statePaths"> The full system paths to the states needing to be tracked. </param>
        /// <param name="animator"> The animator containing the states to track. </param>
        /// <param name="layerIndex"> The layer the states being tracked are on. </param>
        public AnimationStateMapper(List<string> statePaths, Animator animator, int layerIndex)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            this.stateMap = new Dictionary<int, T>();
            this.anim = animator;
            this.layer = layerIndex;
            this.InitializeStateMap(stateMap, statePaths);
        }

        /// <summary> Initiallizes the given map with the meta data on the AnimatorState enum. </summary>
        /// <param name="map"> The map to initialize. </param>
        /// <param name="statePaths"> The full system paths to the states needing to be tracked. </param>
        private void InitializeStateMap(Dictionary<int, T> map, List<string> statePaths)
        {
            Array states = Enum.GetValues(typeof(T));
            for (int i = 0; i < states.Length; i++)
            {
                int hash = Animator.StringToHash(statePaths[i]);
                map.Add(hash, (T)states.GetValue(i));
            }
            if (map.Keys.Count < states.Length)
                Debug.LogError("States are Missing from Behavior stateMap!");
        }

        /// <summary> Returns the AnimatorState enum corresponding to the current state of the provided animator. </summary>
        /// <returns> The AnimatorState for the given hash. </returns>
        public T GetCurrentState()
        {
            int hash = this.anim.GetCurrentAnimatorStateInfo(this.layer).fullPathHash;
            if (this.stateMap.ContainsKey(hash))
                return this.stateMap[hash];
            else
            {
                Debug.LogError("Error: Unable to find given hash in StateMap");
                return new T();
            }
        }

        /// <summary> Gets the current normalized time for the current animation. </summary>
        /// <returns> The current normalized animation time. </returns>
        public float GetCurrentNormalizedTime()
        {
            return this.anim.GetCurrentAnimatorStateInfo(this.layer).normalizedTime % 1f;
        }
    }
}