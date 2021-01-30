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

        [Header("Snap to Ground")]
        /// <summary>
        /// The distance threshold within which Player will snap to the ground if she is close enough.
        /// </summary>
        [Tooltip("The distance threshold within which Player will snap to the ground if she is close enough.")]
        public float snapToGroundRaycastDistance;

        /// <summary>
        /// Which surfaces we consider walkable ground.
        /// </summary>
        [Tooltip("Which surfaces we consider walkable ground.")]
        public LayerMask groundLayerMask;

        [Header("Grounded Check Logic")]

        /// <summary>
        /// How far to check below Player for the ground.
        /// </summary>
        [Tooltip("How far to check below Player for the ground.")]
        public float groundCheckRaycastDistance;

        /// <summary>
        /// How far above Player's position to start our grounded raycast.
        /// </summary>
        [Tooltip("How far above Player's position to start our grounded raycast.")]
        public float groundCheckRaycastYOffset;
    }
}
