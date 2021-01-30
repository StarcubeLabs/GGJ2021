namespace GGJ2021
{
    using UnityEngine;

    public class FollowerPhysics : MonoBehaviour
    {
        enum FollowerStates {Idle, Following, Falling};
        private FollowerStates stateCurr;

        public FollowerConfig config;

        private PositionFollower followScript;

        void Start() {
            stateCurr = FollowerStates.Idle;
            followScript = gameObject.GetComponent<PositionFollower>();
        }

        void Update() {
            if (!followScript.IsNearTarget() && stateCurr != FollowerStates.Falling) {
                stateCurr = FollowerStates.Following;
            } else if (!IsTouchingGround()) {
                stateCurr = FollowerStates.Falling;
            } else {
                stateCurr = FollowerStates.Idle;
            }

            switch (stateCurr) {
                case FollowerStates.Idle:
                    HandleIdleState();
                    break;
                case FollowerStates.Following:
                    followScript.Follow();
                    break;
                case FollowerStates.Falling:
                    ApplyGravity();
                    break;
                default:
                    break;
            }


        }

        void HandleIdleState() {
            
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

        //
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }

        public RaycastHit2D RaycastGround(float distance) {
            float offset = 0.1f;
            Vector3 offsetVec = new Vector3(offset, 0f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offsetVec, Vector2.down, distance + offset, config.groundLayerMask);
            Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
            return hit;
        }
    }
}
