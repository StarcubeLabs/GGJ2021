namespace GGJ2021
{
    using UnityEngine;

    public class PositionRecorder : MonoBehaviour
    {
        [Tooltip("Toggle Recording")]
        public bool isRecording = false; 

        [Tooltip("Which object are we recording")]
        public GameObject target; 

        [Tooltip("How many positions to keep record")]
        public int historySize = 10; 

        [Tooltip("How many seconds between each recording")]
        public float interval = 0;

        public Vector3[] history;
        public int historyIdx; // tracks when to loop over

        private Vector3 previousRecord = Vector3.zero;

        private float nextActionTime = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            historyIdx = 0;
            history = new Vector3[historySize];
        }

        void Update()
        {  
            if (isRecording && Time.time > nextActionTime) {
                nextActionTime += interval;

                AddRecord(Time.deltaTime);
            }
        }

        // Fill array list with Player positions.
        public void AddRecord(float deltaTime)
        {
            // don't record if not much has changed
            if (Vector3.Distance(previousRecord, target.transform.position) < 1.0f) {
                return;
            }

            // record target data
            history[historyIdx] = target.transform.position;
            previousRecord = target.transform.position;

            // set next record index
            if (historyIdx < history.Length - 1)
                historyIdx++;
            else
                historyIdx = 0;
        }
    }
}