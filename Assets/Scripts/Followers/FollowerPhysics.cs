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

        enum FollowerStates {Idling, Falling, Flying, Chasing, Walking, Thinking};
        private FollowerStates stateCurr;
        private FollowerStates statePrev;

        private Vector3 _targetPos;
        public Vector3 targetPos {
            get { 
                if (_targetPos == Vector3.zero) return Vector3.zero;

                return _targetPos + config.targetOffset; 
            }
            set { _targetPos = value; }
        }

        private PositionRecorder positionRecorderScript;

        void Start() {
            stateCurr = FollowerStates.Idling;

            positionRecorderScript = gameObject.GetComponent<PositionRecorder>();
            animator = animatorObj.GetComponent<Animator>();
            playerObj = PlayerController.instance.gameObject;
        }
        void FixedUpdate() {
            if (!isActive) { return; }

            // state handler
            UpdateState();

            // toggle recording state based on how close we are
            positionRecorderScript.isRecording = !IsNearPlayer();
        }
        void Update() {
            switch (stateCurr) {
                case FollowerStates.Chasing:
                case FollowerStates.Walking:
                    animator.Play("JellyWalk");
                    WalkToTarget();
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

            bool isNearTargetButNeedToJump = IsNearTargetButNeedToJump();
            bool isTouchingGround = IsTouchingGround();
            bool doesWantToChase = !IsFalling() && !IsNearPlayer();

            if (isTouchingGround && IsThinking() && !IsNearTarget()) {
                stateNext = FollowerStates.Walking;

            } else if (isNearTargetButNeedToJump && doesWantToChase) {
                stateNext = FollowerStates.Flying;

            } else if (!isNearTargetButNeedToJump && isTouchingGround && doesWantToChase) { // chase player 
                stateNext = FollowerStates.Chasing;

            } else if (!isTouchingGround && !IsChasing() && !IsFlying()) { // gotta fall
                stateNext = FollowerStates.Falling;

            } else if (IsMoving() && !IsWalking() && IsNearPlayer()) { // close enough, think about doing something else
                stateNext = FollowerStates.Thinking;

            } else if (!IsMoving() && IsNearPlayer() && IsNearTarget()) { // no need to do anything
                stateNext = FollowerStates.Idling;

            // maintain movement state
            } else if (IsMoving()) {
                stateNext = stateCurr;
            }
        
            // 
            if (doesWantToChase && (IsChasing() || IsFlying())) {
                UpdateTargetPos();
            }

            // -- 
            // check if no state change
            if (stateNext == stateCurr) {
                return;
            }

            statePrev = stateCurr;
            stateCurr = stateNext;
            print("stateCurr: " + stateCurr + " // statePrev: " + statePrev);

            if (IsThinking()) {
                UpdateTargetPos();
            }
        }
        void WalkToTarget() {
            float step = config.MaxSpeed * Time.deltaTime;

            float nextPosY = targetPos.y > config.flyingDistance ? transform.position.y : targetPos.y;
            Vector3 nextPosition = new Vector3(targetPos.x, nextPosY, 0);

            transform.position = Vector3.Lerp(transform.position, nextPosition, step);
        }
        void FlyToTarget() {
            float step = config.MaxSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, step);
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

            if (IsThinking() && !IsFalling()) {
                return FindHangoutPosition();
            }

            // zero represents no target
            return Vector3.zero;
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
        public bool IsWalking() {
            return stateCurr == FollowerStates.Walking;
        }
        public bool IsMoving() {
            return IsWalking() || IsChasing() || IsFlying();
        }
        public bool IsNearTarget() {
            if (targetPos == Vector3.zero) {
                return true;
            }

            if (Mathf.Abs(transform.position.x - targetPos.x) > config.nearbyDistance.x) {
                return false;
            }

            if (Mathf.Abs(transform.position.y - targetPos.y) > config.nearbyDistance.y) {
                return false;
            }

            return true;
        }
        public bool IsNearPlayer() {
            return Vector3.Distance(transform.position, PlayerPosition()) < config.followMinDistance;
        }
        public bool IsNearTargetButNeedToJump() {
            if (targetPos == Vector3.zero) {
                return true;
            }

            bool isNearbyX = Mathf.Abs(transform.position.x - targetPos.x) > config.nearbyDistance.x;
            if ((targetPos.y - transform.position.y) > config.flyingDistance) {
                return isNearbyX;
            }

            return false;
        }
        public bool IsTouchingGround() {
            return RaycastGround(config.groundCheckRaycastDistance).collider != null;
        }
        public Vector3 PlayerPosition() {
            return playerObj.transform.position;
        }
    }
}
