namespace GGJ2021.Util
{
    using UnityEngine;

    class CameraTracker : MonoBehaviour
    {
        public Transform player;
        public float trackSpeed;
        public Transform upperBound;
        public Transform lowerBound;
        public Transform leftBound;
        public Transform rightBound;

        Vector2 target;

        private Vector3 offset = new Vector3(0, 2);

        void Start()
        {
        }

        void Update()
        {
            if (player == null)
            {
                if (PlayerController.instance == null)
                    return;

                player = PlayerController.instance.transform;
            }

            target = player.position + offset;

            float speed = Time.deltaTime * ((Mathf.Ceil(Vector2.Distance(this.transform.position, target))) 
                + (PlayerController.instance.goingThroughPipe ? PlayerController.instance.currentPipeSpeed : trackSpeed));
            if (target.x > leftBound.transform.position.x && target.x < rightBound.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(target.x, this.transform.position.y, this.transform.position.z), speed);
            else if (target.x < leftBound.transform.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(leftBound.position.x, this.transform.position.y, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(rightBound.position.x, this.transform.position.y, this.transform.position.z), speed);

            if (target.y > lowerBound.transform.position.y && target.y < upperBound.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, target.y, this.transform.position.z), speed);
            else if (target.y < lowerBound.transform.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, lowerBound.position.y, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, upperBound.position.y, this.transform.position.z), speed);
        }

        public void setBounds(Transform leftBound, Transform rightBound, Transform upperBound, Transform lowerBound)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }

        /// <summary> Centers the camera on the player immediately. </summary>
        public void ResetPosition()
        {
            this.transform.position = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;
        }
    }
}
