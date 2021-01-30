namespace GGJ2021
{
    using UnityEngine;

    public class PositionFollower : MonoBehaviour
    {

        public GameObject trackerObject;
        public int positionIdx;

        public float minDistance = 0.002f;
        public float speed = 10.0f;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            PositionTracker trackerScript = trackerObject.GetComponent<PositionTracker>();
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