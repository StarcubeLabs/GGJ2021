namespace GGJ2021
{
    public class PlayerStateMachine
    {
        readonly PlayerController Controller;
        PlayerState CurrentState;
        PlayerState NextState;

        public PlayerStateMachine(PlayerController Controller)
        {
            this.Controller = Controller;

            CurrentState = new PlayerStateIdle(Controller);
        }

        public void OnUpdate(float time)
        {
            CurrentState.OnUpdate(time);
            if (CurrentState.CanExit() && CurrentState.NextState() != null)
            {
                NextState = CurrentState.NextState();
                CurrentState = NextState;
                NextState = null;
            }
        }

        public void OnFixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }

        public string GetCurrentStateName()
        {
            return CurrentState.stateName;
        }
    }
}