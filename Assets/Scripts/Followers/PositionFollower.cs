namespace GGJ2021
{
    using UnityEngine;

    public class PositionFollower : MonoBehaviour
    {

        public GameObject trackerObject;
        // private PlayerController playerControllerScript;
        private PositionTracker trackerScript;

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
        void Update()
        {
            Vector3 targetPos = trackerScript.GetTrackedPosition(positionIdx);

            // print("dist: " + Vector3.Distance(transform.position, targetPos));
            if (Vector3.Distance(transform.position, targetPos) < minDistance)
            {   
                return;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }
}