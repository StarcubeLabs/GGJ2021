namespace GGJ2021
{
    using UnityEngine;

    public class TransitPipeEntrance : MonoBehaviour
    {
        public Pipe.StartingPoint pipeType;
        public Pipe pipe;
        public CollisionWrapper colWrapper;

        private void Start()
        {
            colWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);
            colWrapper.AssignFunctionToTriggerStayDelegate(OnCollision);
        }

        public void OnCollision(Collider2D col)
        {
            Debug.Log("COLLISION");
            PlayerController.instance.OnPipeCollision(pipe, pipeType);
        }
    }
}
