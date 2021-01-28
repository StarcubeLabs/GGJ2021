namespace GGJ2021
{
    using UnityEngine;

    public class SurfaceCollisionEntity
    {
        protected GameObject gameObject;
        protected PhysicsEntity physicsEntity;
        protected CollisionWrapper collisionWrapper;

        //Bools used to toggle which type of check we use to determine the slope of the surface the entity is standing on.
        //Option 1: Use the highest normal value from the ground colliding with the entity.
        protected bool useCollisionNormals;
        //Option 2: Use the averaged normals from a series of raycasts from the entity's feet to the ground.
        protected bool useGroupRaycastNormals;

        protected bool isGrounded;
        protected bool isSliding;

        //Angle Y value of the steepest slope that can cause the player to slide.
        protected float steepestSlopeYAngle;
        //Collision normal direction of the steepest slope, used to calculate which direction the player should slide.
        public Vector2 steepestSlopeNormal { get; private set; }
        //Dot product of the player's forward and the slope normal. When this is greater than 0, it means the player is moving towards the direction the slope is pushing them.
        //When this is the case, we want to apply gravity to prevent the player from jittering as they go downhill.
        public float slopeNormalDotProduct;
        protected float slidingYAngleCutoff;
        protected float groundedYAngleCutoff;
        protected bool canSlide;

        //Variables used to calculate floor normals.
        protected Vector2 raycastOrigin;
        protected RaycastHit hit;
        protected Vector2 hitNormal;
        protected Vector2 averagedHitNormal;

        //Group Raycast vars
        protected float groundCheckRaycastDistance;
        protected float groundCheckRaycastSpread;
        protected float groundCheckCenterWeight;
        protected float groundCheckRaycastYOffset;
        protected LayerMask groundLayerMask;
        protected int numRaycastHits = 0;

        protected FlippableCharacter flippableCharacter;

        protected readonly bool debug = true;

        public SurfaceCollisionEntity(GameObject gameObject, PhysicsEntity physicsEntity, CollisionWrapper collisionWrapper, float groundCheckRaycastDistance, float groundCheckRaycastSpread,
             float groundCheckCenterWeight, float groundCheckRaycastYOffset, LayerMask groundLayerMask, float slidingYAngleCutoff, float groundedYAngleCutoff, FlippableCharacter flippableCharacter,
             bool canSlide = true, bool useGroupRaycastNormals = true, bool useCollisionNormals = false)
        {
            this.gameObject = gameObject;
            this.physicsEntity = physicsEntity;
            this.collisionWrapper = collisionWrapper;
            this.groundCheckRaycastDistance = groundCheckRaycastDistance;
            this.groundCheckRaycastSpread = groundCheckRaycastSpread;
            this.groundCheckCenterWeight = groundCheckCenterWeight;
            this.groundCheckRaycastYOffset = groundCheckRaycastYOffset;
            this.groundLayerMask = groundLayerMask;
            this.slidingYAngleCutoff = slidingYAngleCutoff;
            this.groundedYAngleCutoff = groundedYAngleCutoff;
            this.flippableCharacter = flippableCharacter;
            this.canSlide = canSlide;
            this.useGroupRaycastNormals = useGroupRaycastNormals;
            this.useCollisionNormals = useCollisionNormals;

            if (useCollisionNormals == true)
            {
                this.collisionWrapper.AssignFunctionToCollisionEnterDelegate(SetGroundedFromCollision);
                this.collisionWrapper.AssignFunctionToCollisionStayDelegate(SetGroundedFromCollision);
            }
        }

        public void OnFixedUpdate()
        {
            isGrounded = false;
            isSliding = false;
            slopeNormalDotProduct = 0;
            if (useGroupRaycastNormals == true)
            {
                GetAveragedNormalFromDownwardRaycast(gameObject.transform.position, groundCheckRaycastDistance);
                if (numRaycastHits != 0)
                {
                    SetGroundedFromNormal(averagedHitNormal);
                }
            }
        }

        public Vector2 GetAveragedNormalFromDownwardRaycast(Vector2 position, float raycastDistance)
        {
            averagedHitNormal = Vector2.zero;
            numRaycastHits = 0;
            averagedHitNormal = GetNormalFromRaycast(position, 0f, raycastDistance) * groundCheckCenterWeight +
            GetNormalFromRaycast(position, groundCheckRaycastSpread, raycastDistance) +
            GetNormalFromRaycast(position,-groundCheckRaycastSpread, raycastDistance);
            if (numRaycastHits != 0)
            {
                averagedHitNormal = averagedHitNormal / numRaycastHits;
            }
            return averagedHitNormal;
        }

        protected Vector2 GetNormalFromRaycast(Vector2 origin, float xOffset, float distance)
        {
            raycastOrigin = new Vector2(origin.x + xOffset, origin.y + groundCheckRaycastYOffset);
            Debug.DrawRay(raycastOrigin, Vector2.down * (distance + groundCheckRaycastYOffset), Color.magenta);
            if (Physics.Raycast(raycastOrigin, -Vector2.up, out hit, distance + groundCheckRaycastYOffset, groundLayerMask))
            {
                hitNormal = hit.normal;
                //Only pay attention to collisions that 
                if (Vector2.Angle(hitNormal, Vector2.up) <= slidingYAngleCutoff)
                {
                    numRaycastHits++;
                    return hit.normal;
                }
            }
            return Vector2.zero;
        }

        protected void SetGroundedFromCollision(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                SetGroundedFromNormal(collision.GetContact(i).normal);
            }
        }

        protected void SetGroundedFromNormal(Vector2 normal)
        {
            steepestSlopeYAngle = 0;
            steepestSlopeNormal = Vector2.zero;

            float slopeDownAngle = Vector2.Angle(normal, Vector2.up);

            //If the entity is colliding with something that has a contact normal y angle less than groundedYAngleCutoff, we consider them grounded.
            isGrounded |= slopeDownAngle <= groundedYAngleCutoff;
            //If the entity is colliding with something that has a contact normal y angle less than slopeDownAngle but greater than groundedYAngleCutoff, we consider them sliding.
            isSliding |= slopeDownAngle > groundedYAngleCutoff && slopeDownAngle <= slidingYAngleCutoff;

            //Use the entity's direction for calculating the slope normal dot product.
            Vector2 entityDirection;
            if (flippableCharacter.facingRight)
            {
                entityDirection = new Vector2(1, 0);
            }
            else
            {
                entityDirection = new Vector2(-1, 0);
            }

            //If this slope is steeper than our last one without angling further downwards than sideways, then it is our new steepest slope.
            if (slopeDownAngle > steepestSlopeYAngle && slopeDownAngle < slidingYAngleCutoff)
            {
                steepestSlopeYAngle = slopeDownAngle;
                steepestSlopeNormal = normal;

                slopeNormalDotProduct = Vector2.Dot(entityDirection, steepestSlopeNormal);
            }

            if (debug && (isGrounded || isSliding))
            {
                Debug.DrawRay(gameObject.transform.position, steepestSlopeNormal, Color.yellow);
            }
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }

        public bool IsSliding()
        {
            return isSliding && !isGrounded;
        }

        public bool IsInAir()
        {
            return !isGrounded && !isSliding;
        }
    }
}