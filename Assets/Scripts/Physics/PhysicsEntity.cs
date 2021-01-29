namespace GGJ2021
{
    using UnityEngine;

    public class PhysicsEntity
    {
        public Vector2 velocity = Vector2.zero;
        public Vector2 desiredVelocity = Vector2.zero;
        public Vector2 acceleration = Vector2.zero;
        public float maxSpeedChange;

        public GameObject gameObject;
        public Rigidbody2D rb;

        private float colliderHeight;

        //Create three offsets from the entity's pivot point for generating an upper, central, and lower point to raycast from when checking for wall collisions.
        public Vector2 colliderOffset;
        public Vector2 upperColliderOffset;
        public Vector2 lowerColliderOffset;

        //Raycast origin points for preventing movement into walls.
        public Vector2 colliderCenterPosition;
        public Vector2 colliderUpperPosition;
        public Vector2 colliderLowerPosition;

        //The radius of the entity's collider, added to her velocity when determing the distance to check for wall collisions.
        private float colliderRadius;
        //Used to determine how far from the center of our collider that the upper and lower raycasts should be. 0 for no separation, 1 for the very top and bottom of the entity's collider.
        private float upperLowerYHeightScale;

        //How far the entity is about to travel based on velocity
        public float predictedMovementDistance;

        private RaycastHit2D hit;

        private bool debug = true;

        //The how far above the ground to check for steep slopes when using the ProhibitMovementOntoSteepSlope function.
        private float steepSlopeCheckRaycastDistance;

        public PhysicsEntity(GameObject gameObject, Rigidbody2D rb, Vector2 colliderOffset, float colliderHeight, float colliderRadius, float upperLowerYHeightScale = 0.5f)
        {
            this.gameObject = gameObject;
            this.rb = rb;
            this.colliderOffset = colliderOffset;
            this.colliderRadius = colliderRadius;
            this.upperLowerYHeightScale = upperLowerYHeightScale;
            this.colliderHeight = colliderHeight;
            upperColliderOffset = colliderOffset;
            upperColliderOffset.y += (colliderHeight / 2.0f) * upperLowerYHeightScale;
            lowerColliderOffset = colliderOffset;
            lowerColliderOffset.y += (-colliderHeight / 2.0f) * upperLowerYHeightScale;
            steepSlopeCheckRaycastDistance = colliderHeight * 0.25f;
        }

        public void ResetDesiredVelocity()
        {
            desiredVelocity = Vector2.zero;
        }

        public void OverrideVelocity(Vector2 newVelocity)
        {
            velocity = newVelocity;
            desiredVelocity = newVelocity;
        }

        public void CalculateVelocity(Vector2 direction, float maxSpeed, float maxAcceleration, bool ignoreYValue = true)
        {
            desiredVelocity = new Vector2(direction.x, 0) * maxSpeed;
            maxSpeedChange = maxAcceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            if (ignoreYValue)
            {
                //Keep whatever our rigidbody y velocity was on the last frame to ensure that gravity works properly.
                velocity.y = rb.velocity.y;
            }
            else
            {
                velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
            }
        }

        public void AddForceToVelocity(Vector2 force, bool ignoreYValue = true)
        {
            if (ignoreYValue)
            {
                force.y = 0f;
            }
            velocity = velocity + force;
        }

        public void ApplyVelocityModifier(float modifier)
        {
            desiredVelocity = desiredVelocity * modifier;
            velocity = velocity * modifier;
        }

        public void ApplyVelocity()
        {
            rb.velocity = velocity;
        }

        /// <summary>
        /// One frame function that suddenly launches the entity in a particular direction.
        /// Intended to juice up enemy deaths and such.
        /// </summary>
        /// <param name="direction"> Which direction the entity goes flying in. </param>
        /// <param name="yForce"> How high up the entity gets launched. </param>
        /// <param name="launchSpeed"> How quickly the entity is launched. </param>
        /// <param name="rotationSpeed"> How quickly the entity spins around in the air. </param>
        public void LaunchEntity(Vector2 direction, float yForce, float launchSpeed, float rotationSpeed)
        {
            desiredVelocity = direction.normalized;
            desiredVelocity = new Vector2(desiredVelocity.x, yForce);

            rb.angularVelocity = Random.Range(-1.0f, 1.0f) * rotationSpeed;

            velocity = desiredVelocity * launchSpeed;
        }

        public void ApplyGravity(Vector2 gravity, float maxSpeed, bool isGrounded, float slopeNormalDotProduct, bool isIdle = false)
        {
            if (isGrounded == false)
            {
                //Apply gravity if agent is in the air or sliding.
                rb.AddForce(gravity, ForceMode2D.Force);
            }
            else if (isIdle == false && slopeNormalDotProduct > 0.1f)
            {
                //Apply extra gravity if agent is moving downhill to prevent bouncing.
                rb.AddForce(gravity * 1.5f, ForceMode2D.Force);
            }
            else
            {
                //Don't apply gravity if we are grounded, as this can sometimes lead to sliding when the agent stands on slight slopes.
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }

            //Cap speed after applying gravity when grounded to prevent the entity from moving too quickly downhill.
            if (isGrounded == true)
            {
                CapSpeed(maxSpeed);
            }
        }

        public void InstantFaceDirection(Vector2 direction)
        {
            direction.y = 0f;
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector2.up);
        }

        public void CapSpeed(float maxSpeed)
        {
            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
        }

        private void SetRaycastOriginPoints()
        {
            //Calculate the approximate distance that will be traversed, accounting for the radius of our collider.
            predictedMovementDistance = velocity.magnitude * Time.deltaTime + colliderRadius;

            //Raycast from the top, center, and bottom of the entity's collider to check for potential collisions.
            colliderCenterPosition = (Vector2) gameObject.transform.position + colliderOffset;
            colliderUpperPosition = (Vector2) gameObject.transform.position + upperColliderOffset;
            colliderLowerPosition = (Vector2) gameObject.transform.position + lowerColliderOffset;
        }

        /// <summary>
        /// Used to prevent the entity from walking into walls and halting their descent during a fall.
        /// We shoot three raycasts out from various heights on the entity, using her velocity and collider radius to predict where she will be on the next frame.
        /// If any of these raycast hit an object in layerMask, cancel all horizontal movement.
        /// </summary>
        public void ProhibitMovementIntoWalls(LayerMask layerMask, bool isDash = false)
        {
            SetRaycastOriginPoints();

            //Check if the body's current velocity will result in a collision
            if (Physics2D.Raycast(colliderCenterPosition, velocity.normalized, predictedMovementDistance * 1.0f, layerMask) ||
                Physics2D.Raycast(colliderUpperPosition, velocity.normalized, predictedMovementDistance * 1.0f, layerMask) ||
                (Physics2D.Raycast(colliderLowerPosition, velocity.normalized, predictedMovementDistance * 0.9f, layerMask) && !isDash))
            {
                if (isDash == true)
                {
                    //If the entity dashes into a wall, cancel their movement for the remainder of the dash.
                    velocity = Vector2.zero;
                }
                else
                {
                    //If the entity walks into a wall, stop the horizontal movement
                    IgnoreHorizontalMovementInput();
                }
            }
            if (debug)
            {
                Debug.DrawRay(colliderCenterPosition, velocity.normalized * predictedMovementDistance * 1.0f, Color.yellow);
                Debug.DrawRay(colliderUpperPosition, velocity.normalized * predictedMovementDistance * 1.0f, Color.blue);
                Debug.DrawRay(colliderLowerPosition, velocity.normalized * predictedMovementDistance * 0.9f, Color.green);
            }
        }

        /// <summary>
        /// Used to prevent the entity from walking onto an overly steep slope. This prevents jitteryness from walking up and immediately sliding back down a hill.
        /// </summary>
        /// <param name="preemptiveSurfaceCollisionEntity"> The class used to simulate the steepness of the slope Melody will be standing on if she moves. </param>
        /// <param name="isGrounded"> Whether or not the player is grounded. If they are sliding or in the air, we do not want to apply this function. </param>
        /// <param name="isDash"> Whether or not the player is dashing. This will effect HOW we stop their movement up the slope. </param>
        public void ProhibitMovementOntoSteepSlope(PreemptiveSurfaceCollisionEntity preemptiveSurfaceCollisionEntity, bool isGrounded, bool isInAir, bool isDash = false)
        {
            if (isGrounded || isInAir)
            {
                SetRaycastOriginPoints();

                //The position of the bottom of the player.
                Vector2 playerPos = PlayerController.instance.bottom.transform.position;
                //The vector of movement that will be applied to the player, assuming their movement is valid.
                Vector2 movementDisplacement = velocity.normalized * predictedMovementDistance;
                //Start our raycast from where the player will be if their movement continues.
                Vector2 raycastPos = playerPos + movementDisplacement;
                //Add an offset to the y value so that we hit the ground when we raycast down.
                raycastPos.y += steepSlopeCheckRaycastDistance;

                if (preemptiveSurfaceCollisionEntity.IsMovementDestinationASteepSlope(raycastPos, steepSlopeCheckRaycastDistance))
                {
                    if (isDash == true)
                    {
                        //If the entity dashes into a steep slope, cancel their movement for the remainder of the dash.
                        velocity = Vector2.zero;
                    }
                    else
                    {
                        //If the entity walks into a steep slope, stop the horizontal movement
                        AddForceToVelocity(preemptiveSurfaceCollisionEntity.steepestSlopeNormal * velocity.magnitude);
                    }
                }
            }
        }

        public void ApplyStationaryVelocity()
        {
            velocity = Vector2.zero;
            desiredVelocity = Vector2.zero;
            rb.velocity = velocity;
        }

        public void IgnoreHorizontalMovementInput()
        {
            velocity = new Vector2(0.0f, velocity.y);
        }

        public void ClampUpwardsVelocity()
        {
            velocity = new Vector2(velocity.x, Mathf.Min(velocity.x, 0f));
        }

        public void AddForceToVelocity(Vector2 newForce)
        {
            velocity = velocity + newForce;
        }

        public void SnapToGround(bool isGrounded, float snapToGroundRaycastDistance, LayerMask layerMask)
        {
            if (!isGrounded)
            {
                hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, snapToGroundRaycastDistance, layerMask);
                if (hit.collider != null)
                {
                    rb.MovePosition(hit.point);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            rb.MovePosition(position);
        }

        public void ToggleIsKinematic(bool isKinematic)
        {
            rb.isKinematic = isKinematic;
        }
    }
}
