namespace GGJ2021
{
    using UnityEngine;

    public abstract class PlayerState
    {
        public string stateName;

        private bool isEntering;

        protected bool ableToExit;
        protected PlayerState nextState;
        protected readonly PlayerController melodyController;

        public PlayerState(PlayerController controller)
        {
            melodyController = controller;
            isEntering = true;
            ableToExit = false;
        }

        protected abstract void Enter();

        public virtual void OnUpdate(float time)
        {
            if (isEntering)
            {
                isEntering = false;
                Enter();
            }
        }

        public virtual void OnFixedUpdate()
        {
        }

        public abstract void OnExit();

        public virtual bool CanExit()
        {
            if (ableToExit)
            {
                if (nextState == null)
                {
                    Debug.LogError("State trying to exit, but 'nextState' is not set");
                    return false;
                }
                OnExit();
                return true;
            }
            return ableToExit;
        }

        public virtual PlayerState NextState()
        {
            return nextState;
        }
    }
}
