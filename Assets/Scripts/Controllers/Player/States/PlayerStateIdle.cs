namespace GGJ2021
{
    public class PlayerStateIdle : PlayerState
    {
        public PlayerStateIdle(PlayerController controller) : base(controller) { stateName = "PlayerStateIdle"; }

        protected override void Enter() { }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);

            if (RewiredPlayerInputManager.instance.GetHorizontalMovement() != 0 || RewiredPlayerInputManager.instance.GetVerticalMovement() != 0)
            {
                ableToExit = true;
                nextState = new PlayerStateMoving(playerController);
            }

            if (RewiredPlayerInputManager.instance.R1Down())
            {
                playerController.hamsterManager.ShootProjectile(playerController.transform.position, playerController.lookDirection.normalized * playerController.config.ProjectileSpeed);
            }
        }

        public override void OnFixedUpdate()
        {
            if (playerController.playerCollision.IsGrounded() && !playerController.playerCollision.IsSliding())
            {
                playerController.playerPhysics.IgnoreHorizontalMovementInput();
                playerController.playerPhysics.ApplyVelocity(playerController.config.MaxSpeed);
            }
            playerController.playerPhysics.ApplyGravity(playerController.config.Gravity, true);
            base.OnFixedUpdate();
        }

        public override void OnExit() { }
    }
}
