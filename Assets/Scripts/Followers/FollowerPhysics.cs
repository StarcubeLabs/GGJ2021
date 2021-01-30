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
            if (IsNearTarget() && !IsNearSourceTarget() && !IsFalling()) {
                RefreshTargetPosition();
            }

            if (!IsNearTarget()) {
                stateCurr = FollowerStates.Chasing;
            } else if (!IsTouchingGround()) {
                stateCurr = FollowerStates.Falling;
            } else {
                stateCurr = FollowerStates.Idling;
            }

            // toggle recording state based on how close we are
            positionRecorderScript.isRecording = !IsNearSourceTarget();
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
            bool wasClose = IsNearSourceTarget();

            float step = speed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, step);

            bool isClose = IsNearSourceTarget();

            // check if newly close to the player
            if (!wasClose && isClose) {
                SetHangoutPosition();
            }
        }
        void RefreshTargetPosition() {
            int firstIdx = positionRecorderScript.historyIdx;
            int nextIdx = firstIdx + 1;
            if (nextIdx >= positionRecorderScript.historySize) {
                nextIdx = 0;
            }

            targetPos = positionRecorderScript.history[nextIdx];
        }
        void ApplyGravity() {
            bool isTouchingGround = IsTouchingGround();
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
        void SetHangoutPosition() {
            Vector3 sourceTargetPos = positionRecorderScript.target.transform.position;
            float xOffset = Random.Range(-1.5f, 1.5f);
            targetPos = new Vector3(sourceTargetPos.x + xOffset, sourceTargetPos.y, sourceTargetPos.z);
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
            return Vector3.Distance(targetPos, transform.position) < 0.5f;
        }
        public bool IsNearSourceTarget() {
            return positionRecorderScript.IsNearTarget(0.9f);
        }
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }
    }
}
