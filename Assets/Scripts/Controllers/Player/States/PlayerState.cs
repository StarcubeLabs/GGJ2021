namespace GGJ2021
{
    using UnityEngine;

    public abstract class PlayerState
    {
        public string stateName;

        private bool isEntering;

        public bool ableToExit;
        public PlayerState nextState;
        protected readonly PlayerController playerController;

        public PlayerState(PlayerController controller)
        {
            playerController = controller;
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
