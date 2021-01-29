namespace GGJ2021
{
    using UnityEngine;

    public class PlayerPhysics
    {
        private PlayerController controller;
        private PhysicsEntity physicsEntity;

        private RaycastHit hit;

        public bool isDashing;
        public Vector2 dashDirection;

        public PlayerPhysics(PlayerController controller)
        {
            this.controller = controller;
            physicsEntity = new PhysicsEntity(controller.gameObject, controller.rigidBody, controller.transform.position,
                controller.capsuleCollider.size.y * controller.transform.localScale.y, (controller.capsuleCollider.size.x / 2.0f) * controller.transform.localScale.x);
        }

        public void ResetDesiredVelocity()
        {
            physicsEntity.ResetDesiredVelocity();
        }

        public void CalculateVelocity(float maxSpeed, float maxAcceleration)
        {
            physicsEntity.CalculateVelocity(controller.move, maxSpeed, maxAcceleration);
        }

        public void ApplyVelocity(float maxSpeed)
        {
            if (controller.playerCollision.IsSliding())
            {
                //Do nothing
            }
            else
            {
                physicsEntity.ProhibitMovementIntoWalls(controller.config.prohibitMovementIntoWallsLayerMask);
                physicsEntity.ProhibitMovementOntoSteepSlope(controller.playerCollision.GetPreemptiveSurfaceCollisionEntity(), controller.playerCollision.IsGrounded(), controller.playerCollision.IsInAir());
                physicsEntity.ApplyVelocity();
            }
            //controller.melodyAnimator.SetWalkRun(physicsEntity.desiredVelocity.magnitude / maxSpeed);
            //controller.melodyAnimator.SetStrafeInfo(controller.transform.forward, physicsEntity.velocity);
        }

        public void ApplyDashVelocity(Vector2 dashVelocity)
        {
            if (controller.playerCollision.IsSliding())
            {
                physicsEntity.ApplyStationaryVelocity();
            }
            else
            {
                physicsEntity.velocity = dashVelocity;
                physicsEntity.ProhibitMovementIntoWalls(controller.config.prohibitDashIntoWallsLayerMask, true);
                physicsEntity.ProhibitMovementOntoSteepSlope(controller.playerCollision.GetPreemptiveSurfaceCollisionEntity(), controller.playerCollision.IsGrounded(), true);
                physicsEntity.ApplyVelocity();
            }
        }

        public void SetPosition(Vector2 position)
        {
            physicsEntity.SetPosition(position);
        }

        public void ApplyGravity(Vector2 gravity, bool isIdle = false)
        {
            physicsEntity.ApplyGravity(gravity, controller.config.MaxSpeed, controller.playerCollision.IsGrounded(), controller.playerCollision.GetSlopeNormalDotProduct(), isIdle);
        }

        public void ApplyDashGravity(Vector2 gravity)
        {
            //Apply gravity if Melody is moving downhill.
            if (controller.playerCollision.GetSlopeNormalDotProduct() > 0.1f)
            {
                controller.rigidBody.AddForce(gravity, ForceMode2D.Force);
            }

            //Cap speed after applying gravity when grounded to prevent Melody from moving too quickly downhill.
            if (controller.playerCollision.IsGrounded() == true)
            {
                physicsEntity.CapSpeed(controller.config.DashLength / controller.config.DashTime);
            }
        }

        public void ApplyRampGravity(Vector2 gravity)
        {
            //Apply gravity if Melody is in the air after a ramp jump.
            if (controller.playerCollision.IsInAir() == true)
            {
                controller.rigidBody.AddForce(gravity, ForceMode2D.Force);
            }
        }

        public Vector2 GetVelocity()
        {
            return physicsEntity.velocity;
        }

        public Vector2 GetRigidbodyVelocity()
        {
            return physicsEntity.rb.velocity;
        }

        public void InstantFaceDirection(Vector2 direction)
        {
            physicsEntity.InstantFaceDirection(direction);
        }

        public void SnapToGround()
        {
            physicsEntity.SnapToGround(controller.playerCollision.IsGrounded(), controller.config.snapToGroundRaycastDistance, controller.config.groundLayerMask);
        }

        public void OverrideVelocity(Vector2 newVelocity)
        {
            physicsEntity.OverrideVelocity(newVelocity);
        }

        public void CapSpeed(float maxSpeed)
        {
            physicsEntity.CapSpeed(maxSpeed);
        }

        public void ApplyStationaryVelocity()
        {
            physicsEntity.ApplyStationaryVelocity();
        }

        public void IgnoreHorizontalMovementInput()
        {
            physicsEntity.IgnoreHorizontalMovementInput();
        }

        public void ApplyJump(float jumpVelocity)
        {
            physicsEntity.ApplyJump(jumpVelocity);
        }

        public void ClampUpwardsVelocity()
        {
            physicsEntity.ClampUpwardsVelocity();
        }

        public PhysicsEntity GetPhysicsEntity()
        {
            return physicsEntity;
        }

        public void ToggleIsKinematic(bool isKinematic)
        {
            physicsEntity.ToggleIsKinematic(isKinematic);
        }

        public void SetRigidbodyConstraints(RigidbodyConstraints2D constraints)
        {
            physicsEntity.rb.constraints = constraints;
        }

        public void SetRigidBodyConstraintsToDefault()
        {
            SetRigidbodyConstraints(controller.defaultConstraints);
        }

        public void SetRigidBodyConstraintsToLockAll()
        {
            SetRigidbodyConstraints(RigidbodyConstraints2D.FreezeAll);
        }

        public void SetRigidBodyConstraintsToLockAllButGravity()
        {
            SetRigidbodyConstraints(RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX);
        }

        public void SetRigidBodyConstraintsToLockMovement()
        {
            SetRigidbodyConstraints(RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX);
        }

        public void SetRigidBodyConstraintsToNone()
        {
            SetRigidbodyConstraints(RigidbodyConstraints2D.None);
        }
    }
}
