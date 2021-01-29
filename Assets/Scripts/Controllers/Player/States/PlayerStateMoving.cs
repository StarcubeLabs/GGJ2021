namespace GGJ2021
{
    public class PlayerStateMoving : PlayerState
    {
        public PlayerStateMoving(PlayerController controller) : base(controller) { stateName = "MovingState"; }

        protected override void Enter()
        {

        }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);

            if (playerController.move.magnitude == 0.0f && playerController.playerPhysics.GetVelocity().magnitude < 0.001f)
            {
                ableToExit = true;
                nextState = new PlayerStateIdle(playerController);
            }

            if (RewiredPlayerInputManager.instance.IsFiring())
            {
                playerController.hamsterManager.ShootProjectile(playerController.transform.position, playerController.lookDirection.normalized * playerController.config.ProjectileSpeed);
            }
            if (RewiredPlayerInputManager.instance.IsDashing())
            {
                ableToExit = true;
                nextState = new PlayerStateDash(playerController);
            }
            else if (RewiredPlayerInputManager.instance.IsJumping() && playerController.playerCollision.IsGrounded() == true)
            {
                ableToExit = true;
                nextState = new PlayerStateJump(playerController);
            }

            playerController.playerPhysics.CalculateVelocity(playerController.config.MaxSpeed, playerController.config.MaxAcceleration);
        }

        public override void OnFixedUpdate()
        {
            playerController.playerPhysics.ApplyVelocity(playerController.config.MaxSpeed);
            if (playerController.move.magnitude == 0.0f && playerController.playerCollision.IsGrounded() == true)
            {
                //If there is no controller input and melody is grounded, do not apply gravity. This prevents her from infinitely sliding down hills.
                //Clamp her upwards movement so she doesn't drift upwards while sliding to a stop, though.
                playerController.playerPhysics.ClampUpwardsVelocity();
            }
            else
            {
                playerController.playerPhysics.ApplyGravity(playerController.config.Gravity);
            }
            playerController.playerPhysics.SnapToGround();

            //Debug.Log("Moving State Velocity Magnitude: " + playerController.playerPhysics.velocity.magnitude);

            base.OnFixedUpdate();
        }

        public override void OnExit()
        {

        }
    }
}