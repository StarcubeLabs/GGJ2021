using UnityEngine;

namespace GGJ2021
{
    public class PlayerStatePipe : PlayerState
    {
        public PlayerStatePipe(PlayerController controller) : base(controller)
        {
            stateName = "PlayerStatePipe";
        }

        protected override void Enter()
        {
            playerController.GooBallHandler.StartDash();
            playerController.playerPhysics.ApplyStationaryVelocity();
            playerController.playerPhysics.isDashing = false;
            playerController.physicsCollider.enabled = false;
            nextState = new PlayerStateIdle(playerController);
            Debug.Log("PlayerStatePipe Enter");
        }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);
            if (playerController.goingThroughPipe == false)
            {
                ableToExit = true;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnExit()
        {
            playerController.physicsCollider.enabled = true;
            playerController.GooBallHandler.StopDash();
            Debug.Log("PlayerStatePipe Exit");
        }
    }
}
