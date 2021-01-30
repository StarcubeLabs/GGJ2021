using UnityEngine;

namespace GGJ2021
{
    public class PlayerStateGrapple : PlayerState
    {
        private Vector2 startPos, endPos;
        private float pauseTimer, moveTimer, initMoveTimer;

        public PlayerStateGrapple(PlayerController controller) : base(controller)
        {
            stateName = "PlayerStateGrapple";
            startPos = controller.transform.position;
            endPos = controller.playerGrappleManager.GetPlayerDesition();
            pauseTimer = controller.config.grappleSuccessPauseTime;
            moveTimer = controller.config.grappleTransitionTime;
            initMoveTimer = controller.config.grappleTransitionTime;
        }

        protected override void Enter(){ }

        public override void OnUpdate(float time)
        {
            if (pauseTimer > 0)
            {
                pauseTimer -= time;
            }
            if (moveTimer > 0)
            {
                moveTimer = Mathf.Max(0, moveTimer -= time);
                playerController.transform.position = Vector3.Lerp(startPos, endPos, 1f - moveTimer / initMoveTimer);
            }

            if (moveTimer <= 0f)
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
            playerController.playerGrappleManager.ResetGrapple();
            playerController.playerPhysics.ApplyStationaryVelocity();
        }
    }
}