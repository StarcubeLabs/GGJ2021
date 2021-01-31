namespace GGJ2021
{
    using UnityEngine;

    public class FollowerPhysics : MonoBehaviour
    {
        [Tooltip("Use this if you're doing something different")]
        public bool isActive;

        public GameObject playerObj;

        public GameObject animatorObj;
        private Animator animator;

        public FollowerConfig config;

        public Vector2 velocity = Vector2.zero;

        enum FollowerStates {Idling, Falling, Flying, Chasing, Wandering, Thinking};
        private FollowerStates stateCurr;
        private FollowerStates statePrev;

        private Vector3 _targetPos;
        public Vector3 targetPos {
            get { 
                // if (_targetPos == Vector3.zero) return Vector3.zero;
                return _targetPos + config.targetOffset; 
            }
            set { _targetPos = value; }
        }

        private int playIdx;
        private bool readyForNextTarget = false;
        private bool hasFinishedWandering = true;

        void Start() {
            stateCurr = FollowerStates.Idling;

            animator = animatorObj.GetComponent<Animator>();
            playerObj = PlayerController.instance.gameObject;
        }
        void FixedUpdate() {
            if (!isActive) { return; }

            // state handler
            UpdateState();
        }
        void Update() {
            switch (stateCurr) {
                case FollowerStates.Chasing:
                case FollowerStates.Wandering:
                    animator.Play("JellyWalk");
                    MoveTowardsTarget();
                    break;
                case FollowerStates.Falling:
                    animator.Play("JellyFall");
                    ApplyGravity();
                    break;
                case FollowerStates.Flying:
                    animator.Play("JellyJump");
                    FlyToTarget();
                    break;
                case FollowerStates.Idling:
                    animator.Play("JellyIdle");
                    break;
                case FollowerStates.Thinking:
                default:
                    break;
            }
        }
        void UpdateState() {
            FollowerStates stateNext = FollowerStates.Idling;

            bool isNearTarget = IsNearTarget();
            bool isNearPlayer = IsNearPlayer();
            bool isNearTargetButNeedToJump = IsNearTargetButNeedToJump();
            bool isTouchingGround = IsTouchingGround();
            bool doesWantToChase = !isNearPlayer;
            bool doesWantToThink = false;
            bool doesWantIdle = isNearTarget && isNearPlayer && hasFinishedWandering;

            if (!doesWantToThink && doesWantToChase) {
                if (isNearTargetButNeedToJump) {
                    stateNext = FollowerStates.Flying;
                } else if (!isTouchingGround && IsTargetBelow()) {
                    stateNext = FollowerStates.Falling;
                } else {
                    stateNext = FollowerStates.Chasing;
                }
            } else if (doesWantIdle && !doesWantToThink) {
                stateNext = FollowerStates.Idling;

            } else if (!isTouchingGround && !IsMoving()) {
                stateNext = FollowerStates.Falling;

            } else if (isTouchingGround && isNearPlayer && hasFinishedWandering && !IsIdling()) {
                hasFinishedWandering = false;
                stateNext = FollowerStates.Wandering;

            } else if (doesWantToThink) {
                stateNext = FollowerStates.Thinking;

            // maintain movement state
            } else if (IsMoving()) {
                stateNext = stateCurr;
            }
            
            if (readyForNextTarget) {
                UpdateTargetPos();
                readyForNextTarget = false;
            }

            // check if no state change
            if (stateNext == stateCurr) {
                // print("state keep: " + stateCurr);
                return;
            }

            statePrev = stateCurr;
            stateCurr = stateNext;
            // print("stateCurr: " + stateCurr + " // statePrev: " + statePrev);
        }
        void MoveTowardsTarget() {
            float step = config.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            // reached destination so do something else
            if (IsNearTarget()) {
                if (IsWandering()) {
                    hasFinishedWandering = true;
                } else {
                    readyForNextTarget = true;
                }
            }
        }
        void FlyToTarget() {
            float step = config.flySpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (IsNearTarget()) {
                readyForNextTarget = true;
            }
        }
        void ShouldOverrideTarget() {

        }
        void ApplyGravity() {
            Vector3 currPosition = transform.position;
            Vector3 nextPosition = transform.position - (config.Gravity * Time.deltaTime);

            // check to prevent going below the ground
            Vector2 nearestGroundPos = RaycastGround(config.Gravity.y * 1.1f).point;
            if (nextPosition.y < nearestGroundPos.y) {
                nextPosition.y = nearestGroundPos.y;
            }

            transform.position = nextPosition;

            MoveTowardsTarget();
        }
        void UpdateTargetPos() {
            Vector3 nextTarget = ChooseTarget();

            // only want to update if not zero
            if (nextTarget == Vector3.zero) { return; }

            targetPos = nextTarget;
        }
        Vector3 ChooseTarget() {
            if (!IsNearPlayer()) {
                return PlayerPosition();
            }

            if (!IsFalling() && !IsIdling()) {
                return FindHangoutPosition();
            }

            // zero represents no target
            return transform.position;
        }
        Vector3 FindHangoutPosition() {
            Vector3 pPos = PlayerPosition();
            float xOffset = Random.Range(-2.0f, 2.0f);

            Vector3 nextPos = new Vector3(pPos.x + xOffset, pPos.y, pPos.z);
            // print("FindHangoutPosition() " + nextPos);
            return nextPos;
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
        public bool IsFlying() {
            return stateCurr == FollowerStates.Flying;
        }
        public bool IsIdling() {
            return stateCurr == FollowerStates.Idling;
        }
        public bool IsThinking() {
            return stateCurr == FollowerStates.Thinking;
        }
        public bool IsWandering() {
            return stateCurr == FollowerStates.Wandering;
        }
        public bool IsMoving() {
            return IsWandering() || IsChasing() || IsFlying();
        }
        public bool IsNearTarget() {
            if (targetPos == Vector3.zero) return true;
            return IsNearTargetX() && IsNearTargetY();
        }
        public bool IsNearTargetX() {
            return Mathf.Abs(transform.position.x - targetPos.x) <= config.nearbyDistance.x;
        }
        public bool IsNearTargetY() {
            return Mathf.Abs(transform.position.y - targetPos.y) <= config.nearbyDistance.y;
        }
        public bool IsTargetBelow() {
            if (targetPos == Vector3.zero) return true;
            return transform.position.y > targetPos.y && 
                (Mathf.Abs(transform.position.y - targetPos.y) > config.nearbyDistance.y);
        }
        public bool IsNearPlayer() {
            return Vector3.Distance(transform.position, PlayerPosition()) < config.followMinDistance;
        }
        public bool IsNearTargetButNeedToJump() {
            if (targetPos == Vector3.zero) return false;

            if ((targetPos.y - transform.position.y) > config.flyingDistance) {
                return IsNearTargetX();
            }

            return false;
        }
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }
        public bool IsFarFromGround() {
            return RaycastGround(config.groundFarRaycastDistance).collider == null;
        }
        public Vector3 PlayerPosition() {
            return playerObj.transform.position;
        }
        //--
        void OnDrawGizmos() {
            if (!IsMoving()) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.3f);
        }
    }
}
