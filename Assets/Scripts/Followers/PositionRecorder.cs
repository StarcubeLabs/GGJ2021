﻿namespace GGJ2021
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
        public float interval = 0.1f;

        private Vector3[] history;
        private int historyIdx; // tracks when to loop over

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
                print("recording");
                nextActionTime += interval;

                AddRecord(Time.deltaTime);
            }
        }

        // Fill array list with Player positions.
        public void AddRecord(float deltaTime)
        {
            // record target data
            history[historyIdx] = target.transform.position;

            // set next record index
            if (historyIdx < history.Length - 1)
                historyIdx++;
            else
                historyIdx = 0;
        }

        public bool IsNearTarget(float mindist) {
            return Vector3.Distance(target.transform.position, transform.position) < mindist;
        }
    }
}