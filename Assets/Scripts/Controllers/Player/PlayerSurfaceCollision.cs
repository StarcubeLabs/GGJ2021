namespace GGJ2021
{
    using UnityEngine;

    public class PlayerSurfaceCollision
    {
        //Surface collision for where Melody is currently standing.
        private SurfaceCollisionEntity surfaceCollisionEntity;
        //Surface collision for where Melody is attempting to stand before her velocity is applied.
        //Used to check for and prevent Melody from walking onto steep slopes without changing her actual grounded data.
        private PreemptiveSurfaceCollisionEntity preemptiveSurfaceCollisionEntity;

        public PlayerSurfaceCollision(PlayerController controller)
        {
            surfaceCollisionEntity = new SurfaceCollisionEntity(controller.bottom, controller.playerPhysics.GetPhysicsEntity(), controller.playerPhysicsColliderWrapper, controller.config.groundCheckRaycastDistance,
                controller.config.groundCheckRaycastSpread, controller.config.groundCheckCenterWeight, controller.config.groundCheckRaycastYOffset, controller.config.groundLayerMask,
                controller.config.slidingYAngleCutoff, controller.config.groundedYAngleCutoff, controller, true, true, false);

            preemptiveSurfaceCollisionEntity = new PreemptiveSurfaceCollisionEntity(controller.bottom, controller.playerPhysics.GetPhysicsEntity(), controller.playerPhysicsColliderWrapper, controller.config.groundCheckRaycastDistance,
                controller.config.groundCheckRaycastSpread, controller.config.groundCheckCenterWeight, controller.config.groundCheckRaycastYOffset, controller.config.groundLayerMask,
                controller.config.slidingYAngleCutoff, controller.config.groundedYAngleCutoff, controller, true, true, false);
        }

        public void OnFixedUpdate()
        {
            surfaceCollisionEntity.OnFixedUpdate();
        }

        public bool IsGrounded()
        {
            return surfaceCollisionEntity.IsGrounded();
        }

        public bool IsSliding()
        {
            return surfaceCollisionEntity.IsSliding();
        }

        public bool IsInAir()
        {
            return surfaceCollisionEntity.IsInAir();
        }

        public float GetSlopeNormalDotProduct()
        {
            return surfaceCollisionEntity.slopeNormalDotProduct;
        }

        public bool IsMovementDestinationASteepSlope(Vector3 position, float raycastDistance)
        {
            return preemptiveSurfaceCollisionEntity.IsMovementDestinationASteepSlope(position, raycastDistance);
        }

        public PreemptiveSurfaceCollisionEntity GetPreemptiveSurfaceCollisionEntity()
        {
            return preemptiveSurfaceCollisionEntity;
        }
    }
}
