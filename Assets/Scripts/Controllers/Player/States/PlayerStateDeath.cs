namespace GGJ2021
{
    public class PlayerStateDead : PlayerState
    {
        public PlayerStateDead(PlayerController controller) : base(controller) { stateName = "PlayerStateDead"; }

        protected override void Enter(){ }

        public override void OnUpdate(float time)
        {
            playerController.playerPhysics.ApplyStationaryVelocity();
            base.OnUpdate(time);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnExit() { }
    }
}
