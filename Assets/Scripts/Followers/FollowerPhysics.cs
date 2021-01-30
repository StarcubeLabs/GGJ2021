namespace GGJ2021
{
    using UnityEngine;

    public class FollowerPhysics : MonoBehaviour
    {
        public FollowerConfig config;

        public float speed = 10.0f;

        public Vector2 velocity = Vector2.zero;

        enum FollowerStates {Idling, Falling, Chasing};
        private FollowerStates stateCurr;

        public Vector3 targetPos = Vector3.zero;

        private PositionRecorder positionRecorderScript;

        void Start() {
            stateCurr = FollowerStates.Idling;

            positionRecorderScript = gameObject.GetComponent<PositionRecorder>();
        }

        void FixedUpdate() {
            bool isTouchingGround = IsTouchingGround();
            if (!IsNearTarget() && isTouchingGround) {
                stateCurr = FollowerStates.Chasing;
            } else if (!isTouchingGround) {
                stateCurr = FollowerStates.Falling;
            } else {
                stateCurr = FollowerStates.Idling;
            }

            // toggle recording state based on how close we are
            positionRecorderScript.isRecording = !IsNearTarget();
        }

        void Update() {
            switch (stateCurr) {
                case FollowerStates.Chasing:
                    ChaseTarget();
                    break;
                case FollowerStates.Falling:
                    ApplyGravity();
                    break;
                case FollowerStates.Idling:
                    break;
                default:
                    break;
            }
        }

        void ChaseTarget() {
            if (IsNearTarget()) {   
                return;
            }

            int firstIdx = positionRecorderScript.historyIdx;
            int nextIdx = firstIdx + 1;
            if (nextIdx >= positionRecorderScript.historySize) {
                nextIdx = 0;
            }

            float step = speed * Time.deltaTime;
            // transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            transform.position = Vector3.Lerp(transform.position, positionRecorderScript.history[nextIdx], step);
        }

        void ApplyGravity() {
            bool isTouchingGround = IsTouchingGround();
            print("ApplyGravity()" + isTouchingGround);
            if (!isTouchingGround) {
                Vector3 currPosition = transform.position;
                Vector3 nextPosition = transform.position - (config.Gravity * Time.deltaTime);

                // check to prevent going below the ground
                Vector2 nearestGroundPos = RaycastGround(config.Gravity.y * 1.1f).point;
                if (nextPosition.y < nearestGroundPos.y) {
                    nextPosition.y = nearestGroundPos.y;
                }

                transform.position = nextPosition;
            }
        }

        public RaycastHit2D RaycastGround(float distance) {
            float offset = 0.1f;
            Vector3 offsetVec = new Vector3(offset, 0f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offsetVec, Vector2.down, distance + offset, config.groundLayerMask);
            Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
            return hit;
        }

        // -- utility methods
        public bool IsChasing() {
            return stateCurr == FollowerStates.Chasing;
        }
        public bool IsFalling() {
            return stateCurr == FollowerStates.Falling;
        }
        public bool IsIdling() {
            return stateCurr == FollowerStates.Idling;
        }
        public bool IsNearTarget() {
            return positionRecorderScript.IsNearTarget(0.9f);
        }
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }
    }
}
