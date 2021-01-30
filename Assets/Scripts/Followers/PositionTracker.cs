namespace GGJ2021
{
    using UnityEngine;

    public class PositionTracker : MonoBehaviour
    {
        [Tooltip("list of objects that want to be in this party")]
        public GameObject[] WantsToKnow; 
        private FollowerPhysics[] followerPhysicsScript; 

        [Tooltip("how many seconds between when it updates this position")]
        public float interval = 0.2f;

        private Vector3[] positionHistory;
        private int positionIdx;

        private float nextActionTime = 0.0f;

        private PlayerController playerControllerScript;

        // Start is called before the first frame update
        void Start()
        {
            positionIdx = 0;
            
            int historyLength = WantsToKnow.Length;
            positionHistory = new Vector3[historyLength];
            followerPhysicsScript = new FollowerPhysics[historyLength];
            for (var ii=0; ii<historyLength; ii++) {
                GameObject gameobj = WantsToKnow[ii];
                followerPhysicsScript[ii] = gameobj.GetComponent<FollowerPhysics>();
            }

            playerControllerScript = gameObject.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {  
            if (Time.time > nextActionTime) {
                nextActionTime += interval;

                // if player is not moving, don't add any more positions
                if (playerControllerScript.move.x == 0) {
                    return;
                }

                AddPosition(transform.position);
            }
        }

        void AddPosition(Vector3 newpos) {
            positionHistory[positionIdx] = newpos;
            followerPhysicsScript[positionIdx].targetPos = newpos;

            positionIdx = positionIdx + 1;
            if (positionIdx >= positionHistory.Length) {
                positionIdx = 0;
            }
        }

        public Vector3 GetTrackedPosition(int idx) {
            Vector3 gotten = positionHistory[idx];
            if (gotten == null) {
                return transform.position;
            }

            return gotten;
        }
    }
}