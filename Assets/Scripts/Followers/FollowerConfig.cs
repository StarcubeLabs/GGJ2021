namespace GGJ2021
{
    using UnityEngine;

    [System.Serializable]
    public class FollowerConfig
    {
        [Header("Basic Movement")]
        public float MaxSpeed;         //Unity Units Per Second
        public float MaxAcceleration;  //Unity Units Per Second
        public Vector3 Gravity;

        [Header("Jump Settings")]
        public float jumpSpeed;
        public float jumpTime;

        [Header("Follow Settings")]
        [Tooltip("Distance to be considered close to an object")]
        public Vector3 nearbyDistance = new Vector3(0.5f, 0.2f, 0);
        [Tooltip("Amount to offset from the player when tracking it")]
        public Vector3 targetOffset = new Vector3(0, -0.6f, 0);
        [Tooltip("How close to target before we stop following")]
        public float targetMinDistance = 0.9f;
        [Tooltip("How far from player before starting to chase after it.")]
        public float followMinDistance = 1.5f;

        [Header("Ground")]
        [Tooltip("The distance threshold within which Player will snap to the ground if she is close enough.")]
        public float snapToGroundRaycastDistance;
        [Tooltip("How far to check below Player for the ground.")]
        public float groundCheckRaycastDistance;
        [Tooltip("How far above Player's position to start our grounded raycast.")]
        public float groundCheckRaycastYOffset;

        [Tooltip("Which surfaces we consider walkable ground.")]
        public LayerMask groundLayerMask;
    }
}
