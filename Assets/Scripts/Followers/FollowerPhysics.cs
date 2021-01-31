namespace GGJ2021
{
    using UnityEngine;

    public class FollowerPhysics : MonoBehaviour
    {
        [Tooltip("Use this if you're doing something different")]
        public bool isActive;

        public GameObject sourceObj;

        public GameObject animatorObj;
        public Sprite defaultSprite;
        public Sprite jumpSprite;
        private Animator animator;
        private SpriteRenderer spriter;

        public FollowerConfig config;

        public enum FollowerStates {Idling, Falling, Flying, Chasing, Wandering, Thinking};
        public FollowerStates stateCurr; // public just for the sake of debugging
        private FollowerStates statePrev;

        private Vector3 _prevTargetPos;
        private Vector3 _targetPos; // public just for the sake of debugging
        private Vector3 targetPos {
            get { 
                if (_targetPos == Vector3.zero) return Vector3.zero;
                return _targetPos + config.targetOffset; 
            }
            set { 
                _prevTargetPos = _targetPos;
                _targetPos = value;
            }
        }

        private string _prevAnimName;
        private string _animName;
        private string animName {
            get { return _animName; }
            set {
                _prevAnimName = _animName;
                _animName = value;
            }
        }
        private bool isFacingLeft = true;

        private bool readyForNextTarget = false;
        private bool doesWantToWander = true;
        private bool isMoveAdjusting = false;
        private bool didJustAdjust = false;

        void Start() {
            stateCurr = FollowerStates.Idling;

            animator = animatorObj.GetComponent<Animator>();
            spriter = animatorObj.GetComponentInChildren<SpriteRenderer>();

            animName = "JellyIdle";
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
                    MoveTowardsTarget();
                    break;
                case FollowerStates.Falling:
                    ApplyGravity();
                    break;
                case FollowerStates.Flying:
                    FlyToTarget();
                    break;
                case FollowerStates.Idling:
                    // animator.Play("JellyIdle");
                    break;
                case FollowerStates.Thinking:
                default:
                    break;
            }

            if (animName == "JellyJump") {
                spriter.sprite = jumpSprite;
            } else {
                spriter.sprite = defaultSprite;
            }
            // spriter.transform.rotation = Quaternion.identity;
            animator.Play(animName);
        }
        void UpdateState() {
            FollowerStates stateNext = FollowerStates.Idling;

            bool isNearTarget = IsNearTarget();
            bool isNearSource = IsNearSource();
            bool isTouchingGround = IsTouchingGround();
            bool doesWantToChase = !isNearSource;
            bool doesWantIdle = isNearTarget && (isNearSource || IsNearPlayer() || didJustAdjust) && !doesWantToWander && !isMoveAdjusting;

            // UGHH sorry this is undeciferable
            if (IsVeryFarFromPlayer()) {
                stateNext = FollowerStates.Flying;
                animName = "JellyJump";

            } else if (!doesWantIdle && doesWantToChase) {
                if (!isTouchingGround && !IsTargetBelow()) {
                    stateNext = FollowerStates.Flying;
                    animName = "JellyJump";
                } else if (IsTargetBelow()) {
                    stateNext = FollowerStates.Falling;
                    animName = "JellyFall";
                } else {
                    stateNext = FollowerStates.Chasing;
                    animName = "JellyWalk";
                }

                doesWantToWander = config.isAllowedWandering;

            } else if (!isTouchingGround && !IsMoving() && !didJustAdjust) {
                stateNext = FollowerStates.Falling;
                animName = "JellyFall";

            } else if (isTouchingGround && !doesWantToChase && doesWantToWander) {
                stateNext = FollowerStates.Wandering;
                animName = "JellyWalk";

            } else if (doesWantIdle) {
                stateNext = FollowerStates.Idling;
                animName = "JellyIdle";

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
                return;
            }

            spriter.transform.rotation = Quaternion.identity;
            statePrev = stateCurr;
            stateCurr = stateNext;
            // print("stateCurr: " + stateCurr + " // statePrev: " + statePrev);
        }
        void MoveTowardsTarget() {
            if (targetPos.x > transform.position.x && isFacingLeft) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                isFacingLeft = false;
            } else if (targetPos.x < transform.position.x && !isFacingLeft) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                isFacingLeft = true;
            }

            float step = config.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (IsNearTarget()) {
                if (IsWandering()) {
                    doesWantToWander = false;
                } else {
                    readyForNextTarget = true;
                }

                if (isMoveAdjusting) {
                    didJustAdjust = true;
                } else {
                    didJustAdjust = false;
                }
                isMoveAdjusting = false;
            }
        }
        void FlyToTarget() {
            float step = config.flySpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (IsNearTarget()) {
                readyForNextTarget = true;

                if (isMoveAdjusting) {
                    didJustAdjust = true;
                } else {
                    didJustAdjust = false;
                }
                isMoveAdjusting = false;
            }
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
            // if source is stationary and we are falling,
            // prob means we need to find somewhere to stand
            if (IsFalling() && !isMoveAdjusting && !didJustAdjust) {
                // isMoveAdjusting = true;
                // return FindStablePosition();
            }

            if (!IsNearSource() && !didJustAdjust) {
                return SourcePosition();
            }

            if (doesWantToWander && !didJustAdjust) {
                return FindWanderPosition();
            }

            return transform.position;
        }
        Vector3 FindStablePosition() {
            float testDist = 3.0f;
            Vector3 nextPos = new Vector3(PlayerPosition().x, PlayerPosition().y + 2.5f, 0);

            RaycastHit2D hit = Physics2D.Raycast(nextPos, Vector2.down, testDist, config.groundLayerMask);
            Debug.DrawRay(transform.position, Vector2.down * testDist, Color.yellow);

            if (hit.collider == null) {
                nextPos.y = PlayerPosition().y - 0.02f;
                return nextPos;
            }

            return hit.point;
        }
        Vector3 FindWanderPosition() {
            Vector3 pPos = SourcePosition();
            float xOffset = Random.Range(-config.wanderRange, config.wanderRange);

            Vector3 nextPos = new Vector3(pPos.x + xOffset, pPos.y, pPos.z);
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
        public bool IsTargetSame() {
            return targetPos == _prevTargetPos;
        }
        public bool IsNearSource() {
            return Vector3.Distance(transform.position, SourcePosition()) < config.followMinDistance;
        }
        public bool IsNearPlayer() {
            return Vector3.Distance(transform.position, PlayerPosition()) < config.followMinDistance;
        }
        public bool IsVeryFarFromPlayer() {
            return Vector3.Distance(transform.position, PlayerPosition()) > config.followMaxDistance;
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
        public Vector3 SourcePosition() {
            return sourceObj.transform.position;
        }
        public Vector3 PlayerPosition() {
            return PlayerController.instance.transform.position;
        }
        //--
        void OnDrawGizmos() {
            // if (!IsMoving() && !IsFalling()) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetPos, 0.3f);
        }
    }
}
