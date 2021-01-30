namespace GGJ2021
{
    using UnityEngine;

    [System.Serializable]
    public class PlayerConfig
    {
        [Header("Basic Movement")]
        public float MaxSpeed;         //Unity Units Per Second
        public float MaxAcceleration;  //Unity Units Per Second
        public Vector3 Gravity;

        public Vector3 GroundedDashGravity;

        [Header("Jump Settings")]
        public float jumpSpeed;
        public float jumpTime;

        [Header("Grapple Settings")]
        public float grappleRange;
        public float grappleScatterOffset;
        public float grappleFailRetractTime;
        public float grappleSuccessPrePauseTime;
        public float grappleTransitionTime;
        public float grappleSuccessPostPauseTime;
        public LayerMask grappleLayerMask;
        public AnimationCurve grappleCurve;

        [Header("Reticule Settings")]
        public AnimationCurve reticuleCurve;

        [Header("Projectile Settings")]
        public float ProjectileSpeed;
        public float ProjectileRotationSpeedLowerBound;
        public float ProjectileRotationSpeedUpperBound;
        public Vector3 ProjectileGravity;

        [Header("Dash Settings")]
        public float DashLength;
        public float DashTime;

        public float DashOutroMaxSpeed;
        public Vector3 DashOutroGravity;

        public float dashCooldown;

        [Header("Snap to Ground")]
        /// <summary>
        /// The distance threshold within which Player will snap to the ground if she is close enough.
        /// </summary>
        [Tooltip("The distance threshold within which Player will snap to the ground if she is close enough.")]
        public float snapToGroundRaycastDistance;

        /// <summary>
        /// The distance threshold within which Player will snap to the ground when dashing if she is close enough.
        /// </summary>
        [Tooltip("The distance threshold within which Player will snap to the ground when dashing if she is close enough.")]
        public float snapToGroundRaycastDistanceDash;

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

        /// <summary>
        /// How far out from the central ground check raycast our other ground check raycasts should be. These are all averaged out to get a slope.
        /// </summary>
        [Tooltip("How far out from the central ground check raycast our other ground check raycasts should be. These are all averaged out to get a slope.")]
        public float groundCheckRaycastSpread;

        /// <summary>
        /// The weight of the center raycast. Can be used to skew the average normal in favor of the middle of Player.
        /// </summary>
        [Tooltip("The weight of the center raycast. Can be used to skew the average normal in favor of the middle of Player.")]
        public float groundCheckCenterWeight;

        /// <summary>
        /// If Player is colliding with something that has a contact normal y angle less than groundedYAngleCutoff, we consider her grounded.
        /// </summary>
        [Tooltip("If Player is colliding with something that has a contact normal y angle less than groundedYAngleCutoff, we consider her grounded.")]
        public float groundedYAngleCutoff;

        /// <summary>
        /// If Player is colliding with something that has a contact normal y angle less than slidingYAngleCutoff but greater than groundedYAngleCutoff, we consider her sliding.
        /// </summary>
        [Tooltip("If Player is colliding with something that has a contact normal y angle less than slidingYAngleCutoff but greater than groundedYAngleCutoff, we consider her sliding.")]
        public float slidingYAngleCutoff;

        [Header("Prohibit Movement Into Walls")]

        /// <summary>
        /// Which surfaces will cancel Player's horizontal velocity if she walks into them. Used to prevent sticking to objects by walking into them when falling.
        /// </summary>
        [Tooltip("Which surfaces will cancel Player's horizontal velocity if she walks into them. Used to prevent sticking to objects by walking into them when falling.")]
        public LayerMask prohibitMovementIntoWallsLayerMask;

        /// <summary>
        /// Which surfaces will cancel Player's horizontal velocity if she dashes into them.
        /// </summary>
        [Tooltip("Which surfaces will cancel Player's horizontal velocity if she dashes into them.")]
        public LayerMask prohibitDashIntoWallsLayerMask;

        void Start()
        {
            //TODO load values in depending on save data.
        }
    }
}
