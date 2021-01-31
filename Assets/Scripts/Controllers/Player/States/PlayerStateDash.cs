namespace GGJ2021
{
    using UnityEngine;

    public class PlayerStateDash : PlayerState
    {
        protected Vector2 dodge;
        protected float timer, maxTimer;
        private float dodgeMultiplier;

        private Vector2 normalizedVelocity;
        private float velocityMagnitude;

        public PlayerStateDash(PlayerController controller) : base(controller)
        {
            stateName = "PlayerStateDash";
            timer = 0.0f;
            maxTimer = playerController.config.DashTime;
            controller.GooBallHandler.StartDash();
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_dash");
            FollowerManager.instance.RemoveAbilityHamster(Ability.Dash);
        }

        protected override void Enter()
        {
            dodgeMultiplier = (playerController.config.DashLength / playerController.config.DashTime);

            if (playerController.facingRight)
            {
                dodge = Vector2.right * dodgeMultiplier;
            }
            else
            {
                dodge = Vector2.left * dodgeMultiplier;
            }

            nextState = new PlayerStateIdle(playerController);
            playerController.playerPhysics.ApplyStationaryVelocity();

            timer = 0;

            playerController.playerPhysics.isDashing = true;
            playerController.playerPhysics.dashCooldown = playerController.config.dashCooldown;
            playerController.playerPhysics.dashDirection = dodge;
        }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);

            timer += time;
            if (timer >= maxTimer)
            {
                ableToExit = true;
            }
        }

        public override void OnFixedUpdate()
        {
            //Restrict the Y axis range of Melody's dash once she leaves the ground.
            if (playerController.playerCollision.IsInAir())
            {
                normalizedVelocity = playerController.playerPhysics.GetRigidbodyVelocity().normalized;
                velocityMagnitude = playerController.playerPhysics.GetRigidbodyVelocity().magnitude;
                normalizedVelocity.y = 0f;
                dodge.y = Mathf.Max(normalizedVelocity.y * velocityMagnitude, 0f);
            }
            playerController.playerPhysics.dashDirection = dodge;

            playerController.playerPhysics.ApplyDashVelocity(dodge);
            playerController.playerPhysics.ApplyDashGravity(playerController.config.GroundedDashGravity);
            playerController.playerPhysics.SnapToGround();

            base.OnFixedUpdate();
        }

        public override void OnExit()
        {
            playerController.playerPhysics.isDashing = false;
            playerController.playerPhysics.ApplyStationaryVelocity();
            playerController.GooBallHandler.StopDash();
            playerController.SpawnDashExplosion();
            //FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_explosion_wet");
            FollowerManager.instance.AddAbilityHamster(Ability.Dash);
        }
    }
}
