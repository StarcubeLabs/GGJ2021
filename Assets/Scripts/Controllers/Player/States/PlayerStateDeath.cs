namespace GGJ2021
{
    public class PlayerStateDead : PlayerState
    {
        public PlayerStateDead(PlayerController controller) : base(controller) { stateName = "PlayerStateDead"; }

        protected override void Enter(){ }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);
        }

        public override void OnFixedUpdate()
        {
            if (playerController.playerCollision.IsGrounded() == false)
            {
                playerController.playerPhysics.IgnoreHorizontalMovementInput();
                playerController.playerPhysics.CalculateVelocity(playerController.config.MaxSpeed, playerController.config.MaxAcceleration);
                playerController.playerPhysics.ApplyVelocity(playerController.config.MaxSpeed);
                playerController.playerPhysics.ApplyGravity(playerController.config.Gravity);
            }
            else
            {
                playerController.playerPhysics.ApplyStationaryVelocity();
            }
            base.OnFixedUpdate();
        }

        public override void OnExit() { }
    }
}
