namespace GGJ2021
{
    public class PlayerStateGrapple : PlayerState
    {
        public PlayerStateGrapple(PlayerController controller) : base(controller) { stateName = "PlayerStateGrapple"; }

        protected override void Enter() { }

        public override void OnUpdate(float time)
        {
            base.OnUpdate(time);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnExit() { }
    }
}