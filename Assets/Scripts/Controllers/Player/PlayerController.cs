namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : FlippableCharacter
    {
        public static PlayerController instance;

        public PlayerPhysics playerPhysics;
        public PlayerSurfaceCollision playerCollision;
        public PlayerGrappleManager playerGrappleManager;
        public PlayerHealth playerHealth;
        public PlayerAnimationController playerAnimationController;
        public GooBallHandler GooBallHandler;
        public PlayerConfig config;

        public HamsterManager hamsterManager;

        public CollisionWrapper playerPhysicsColliderWrapper;
        public CollisionWrapper playerHurtboxColliderWrapper;
        public CapsuleCollider2D capsuleCollider;

        [HideInInspector]
        public Vector2 move = new Vector2();
        public Vector2 lookDirection = new Vector2();
        public List<AimingReticule> reticules;

        public GameObject astronaut;
        public List<SpriteRenderer> astronautSprites;
        public GameObject cannon;
        public GameObject gooball;
        private Vector3 originalAstronautScale;
        private Vector3 originalGooballScale;
        public GameObject bottom;

        public LineRenderer grapple;

        [HideInInspector]
        public Rigidbody2D rigidBody;
        [HideInInspector]
        public RigidbodyConstraints2D defaultConstraints;

        public PlayerStateMachine StateMachine;
        public string currentStateName;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            defaultConstraints = rigidBody.constraints;
            originalAstronautScale = astronaut.transform.localScale;
            originalGooballScale = gooball.transform.localScale;

            StateMachine = new PlayerStateMachine(this);
            playerPhysics = new PlayerPhysics(this);
            playerCollision = new PlayerSurfaceCollision(this);
            playerHealth = new PlayerHealth(this, playerHurtboxColliderWrapper);
            playerGrappleManager = new PlayerGrappleManager(this, grapple);
        }

        // Update is called once per frame
        void Update()
        {
            playerPhysics.OnUpdate();
            playerGrappleManager.OnUpdate(Time.deltaTime);
            playerHealth.OnUpdate(Time.deltaTime);
            if (!playerHealth.dead)
            {
                CheckInputs();
            }
            AimReticule();
            StateMachine.OnUpdate(Time.deltaTime);
            currentStateName = StateMachine.GetCurrentStateName();
        }

        void FixedUpdate()
        {
            StateMachine.OnFixedUpdate();
            playerCollision.OnFixedUpdate();
            playerAnimationController.SetInAir(playerCollision.IsInAir());
            playerAnimationController.SetIsWalk(move.x != 0);
        }

        private void CheckInputs()
        {
            move.x = RewiredPlayerInputManager.instance.GetHorizontalMovement();
            if (move.x != 0)
            {
                if (move.x > 0)
                {
                    astronaut.transform.localScale = new Vector3(-originalAstronautScale.x, originalAstronautScale.y, originalAstronautScale.z);
                    gooball.transform.localScale = new Vector3(-originalGooballScale.x, originalGooballScale.y, originalGooballScale.z);
                    facingRight = true;
                }
                else
                {
                    astronaut.transform.localScale = new Vector3(originalAstronautScale.x, originalAstronautScale.y, originalAstronautScale.z);
                    gooball.transform.localScale = new Vector3(originalGooballScale.x, originalGooballScale.y, originalGooballScale.z);
                    facingRight = false;
                }
            }
        }

        private void AimReticule()
        {
            lookDirection = new Vector2(RewiredPlayerInputManager.instance.GetHorizontalMovement2(), RewiredPlayerInputManager.instance.GetVerticalMovement2());
            if (playerHealth.dead)
            {
                lookDirection = new Vector2(0f, 0f);
            }
            reticules.ForEach(p =>
            {
                p.transform.localPosition = lookDirection * p.maxDistance;
                p.currentDistance = (lookDirection * p.maxDistance).magnitude;
            });
            if (lookDirection == Vector2.zero)
            {
                if (facingRight)
                {
                    lookDirection = new Vector2(1f, 0f);
                }
                else
                {
                    lookDirection = new Vector2(-1f, 0f);
                }
            }
            lookDirection = lookDirection.normalized;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * -lookDirection;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            cannon.transform.rotation = targetRotation;
        }
    }
}