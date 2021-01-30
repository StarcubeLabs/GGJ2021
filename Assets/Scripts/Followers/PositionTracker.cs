namespace GGJ2021
{
    using UnityEngine;

    public class PositionTracker : MonoBehaviour
    {
        private Vector3[] positionHistory;
        private int positionIdx;

        private float nextActionTime = 0.0f;
        public float interval = 0.2f;

        // Start is called before the first frame update
        void Start()
        {
            positionHistory = new Vector3[3];
            positionIdx = 0;
        }

        // Update is called once per frame
        void Update()
        {  
            if (Time.time > nextActionTime ) {
                nextActionTime += interval;

                AddPosition(transform.position);
            }
        }

        void AddPosition(Vector3 newpos) {
            positionHistory[positionIdx] = newpos;

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