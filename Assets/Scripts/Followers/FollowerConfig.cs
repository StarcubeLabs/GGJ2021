namespace GGJ2021
{
    using UnityEngine;

    [System.Serializable]
    public class FollowerConfig
    {
        [Header("Basic Movement")]
        public float speed;
        // public float maxSpeedDelta;
        public Vector3 Gravity;

        [Header("Flying Settings")]
        public float flySpeed;
        [Tooltip("How far from target vertically before we start flying")]
        public float flyingDistance;

        [Header("Follow Settings")]
        [Tooltip("Distance to be considered close to an object")]
        public Vector3 nearbyDistance = new Vector3(0.5f, 0.2f, 0);
        [Tooltip("Amount to offset from the player when tracking it")]
        public Vector3 targetOffset = new Vector3(0, -1.1f, 0);
        [Tooltip("How far from player before starting to chase after it.")]
        public float followMinDistance = 1.5f;
        [Tooltip("How far from source before it is TOO far and we just hack it over.")]
        public float followMaxDistance = 5f;
        [Tooltip("Allow Wandering")]
        public bool isAllowedWandering = false;
        [Tooltip("Amount to Wander around")]
        public float wanderRange = 1.5f;

        [Header("Ground")]
        // [Tooltip("The distance threshold within which Player will snap to the ground if she is close enough.")]
        // public float snapToGroundRaycastDistance;
        [Tooltip("How far to check below Player for the ground.")]
        public float groundCheckRaycastDistance;
        [Tooltip("How far to check below Player to be considered Far from ground")]
        public float groundFarRaycastDistance = 1f;

        [Tooltip("Which surfaces we consider walkable ground.")]
        public LayerMask groundLayerMask;
    }
}
