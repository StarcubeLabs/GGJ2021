﻿namespace GGJ2021
{
    public class PlayerStateJump : PlayerState
    {
        private float jumpTimer;

        public PlayerStateJump(PlayerController controller) : base(controller)
        {
            stateName = "PlayerStateJump";
            jumpTimer = playerController.config.jumpTime;
            controller.playerAnimationController.JumpTrigger();
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("player_jump");
            controller.SpawnJumpDust();
        }

        protected override void Enter(){ }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);

            jumpTimer -= time;
            if (jumpTimer <= 0)
            {
                ableToExit = true;
                nextState = new PlayerStateIdle(playerController);
            }

            if (RewiredPlayerInputManager.instance.IsFiring() && playerController.hamsterManager.CanFire())
            {
                playerController.hamsterManager.ShootProjectile(playerController.transform.position, playerController.lookDirection.normalized * playerController.config.ProjectileSpeed);
            }
            if (RewiredPlayerInputManager.instance.IsDashing() && playerController.playerPhysics.CanDash())
            {
                ableToExit = true;
                nextState = new PlayerStateDash(playerController);
            }

            if (RewiredPlayerInputManager.instance.IsGrappling() && playerController.playerGrappleManager.CanGrapple())
            {
                if (playerController.playerGrappleManager.TryGrapple())
                {
                    ableToExit = true;
                    nextState = new PlayerStateGrapple(playerController);
                }
            }
        }

        public override void OnFixedUpdate()
        {
            playerController.playerPhysics.CalculateVelocity(playerController.config.MaxSpeed, playerController.config.MaxAcceleration);
            playerController.playerPhysics.ApplyJump(playerController.config.jumpSpeed);

            playerController.playerPhysics.ApplyVelocity(playerController.config.MaxSpeed);
            base.OnFixedUpdate();
        }

        public override void OnExit() { }
    }
}
