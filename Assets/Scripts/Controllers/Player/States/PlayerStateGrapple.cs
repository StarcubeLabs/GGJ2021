using UnityEngine;

namespace GGJ2021
{
    public class PlayerStateGrapple : PlayerState
    {
        private Vector2 startPos, endPos;
        private float pauseTimer, moveTimer, initMoveTimer, exitTimer;

        public PlayerStateGrapple(PlayerController controller) : base(controller)
        {
            stateName = "PlayerStateGrapple";
            startPos = controller.transform.position;
            endPos = controller.playerGrappleManager.GetPlayerDesition();
            pauseTimer = controller.config.grappleSuccessPrePauseTime;
            exitTimer = controller.config.grappleSuccessPostPauseTime;
            moveTimer = controller.config.grappleTransitionTime;
            initMoveTimer = controller.config.grappleTransitionTime;
            controller.playerPhysics.isGrappling = true;
            controller.playerAnimationController.FireGrapple();
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_grapple_hit");
        }

        protected override void Enter(){ }

        public override void OnUpdate(float time)
        {
            if (pauseTimer > 0f)
            {
                pauseTimer -= time;
                if (pauseTimer <= 0f)
                {
                    FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_grapple_zip");
                }
            }
            else if (moveTimer > 0f)
            {
                moveTimer = Mathf.Max(0f, moveTimer -= time);
                float t = 1f - moveTimer / initMoveTimer;
                t = playerController.config.grappleCurve.Evaluate(t);
                playerController.transform.position = Vector3.Lerp(startPos, endPos, t);
            }
            else if (exitTimer > 0f)
            {
                playerController.playerPhysics.ApplyStationaryVelocity();
                exitTimer -= time;

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

            if (exitTimer <= 0f)
            {
                ableToExit = true;
                nextState = new PlayerStateIdle(playerController);
            }

            playerController.playerGrappleManager.DrawGrappleSuccess();

            base.OnUpdate(time);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnExit()
        {
            playerController.playerPhysics.isGrappling = false;
            playerController.playerGrappleManager.ResetGrapple();
            playerController.playerPhysics.ApplyStationaryVelocity();
        }
    }
}