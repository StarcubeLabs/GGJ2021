namespace GGJ2021
{
    using UnityEngine;

    public class FollowerPhysics : MonoBehaviour
    {
        public FollowerConfig config;

        public float minDistance = 0.005f;
        public float speed = 10.0f;

        enum FollowerStates {Idling, Falling, Chasing};
        private FollowerStates stateCurr;

        public Vector3 targetPos = Vector3.one;

        void Start() {
            stateCurr = FollowerStates.Idling;
        }

        void Update() {
            bool isTouchingGround = IsTouchingGround();
            if (!IsNearTarget() && isTouchingGround) {
                stateCurr = FollowerStates.Chasing;
            } else if (!isTouchingGround) {
                stateCurr = FollowerStates.Falling;
            } else {
                stateCurr = FollowerStates.Idling;
            }

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

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
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
            return Mathf.Abs(transform.position.x - targetPos.x) < minDistance;
        }
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }
    }
}
