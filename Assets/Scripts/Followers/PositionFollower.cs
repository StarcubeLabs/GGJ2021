namespace GGJ2021
{
    using UnityEngine;

    public class PositionFollower : MonoBehaviour
    {

        public GameObject trackerObject;
        // private PlayerController playerControllerScript;
        private PositionTracker trackerScript;
        private Vector3 targetPos;

        public int positionIdx;

        public float minDistance = 0.005f;
        public float speed = 10.0f;

        // Start is called before the first frame update
        void Start()
        {
            // playerControllerScript = trackerObject.GetComponent<PlayerController>();
            trackerScript = trackerObject.GetComponent<PositionTracker>();
        }

        // Update is called once per frame
        void Update() {
            // gonna constantly update the tracked target
            targetPos = trackerScript.GetTrackedPosition(positionIdx);
        }

        public bool IsNearTarget() {
            // return Vector3.Distance(transform.position, targetPos) < minDistance;
            return Mathf.Abs(transform.position.x - targetPos.x) < minDistance;
        }

        public void Follow() {

            // print("dist: " + Vector3.Distance(transform.position, targetPos));
            if (IsNearTarget()) {   
                return;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }
}