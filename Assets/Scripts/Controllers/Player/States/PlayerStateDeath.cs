using GGJ2021.Management;

namespace GGJ2021
{
    public class PlayerStateDead : PlayerState
    {
        private const float DEATH_TIME = 1.5f;
        private float deathTimer;

        public PlayerStateDead(PlayerController controller) : base(controller) { stateName = "PlayerStateDead"; }

        protected override void Enter()
        {
            deathTimer = DEATH_TIME;
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("player_death");
        }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);
            deathTimer -= time;
            if (deathTimer <= 0)
            {
                PlayerController.instance.playerHealth.RestoreHealthToFull();
                PlayerController.instance.GetComponentInChildren<SceneTransition>().Transition();
            }
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
