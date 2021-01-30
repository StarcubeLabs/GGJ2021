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
        public PlayerConfig config;

        public HamsterManager hamsterManager;

        public CollisionWrapper playerPhysicsColliderWrapper;
        public CollisionWrapper playerHurtboxColliderWrapper;
        public CapsuleCollider2D capsuleCollider;

        [HideInInspector]
        public Vector2 move = new Vector2();
        public Vector2 lookDirection = new Vector2();
        public List<AimingReticule> reticules;

        public GameObject playerSprites;
        private Vector3 originalSpritesScale;
        public GameObject bottom;

        public LineRenderer grapple;

        [HideInInspector]
        public Rigidbody2D rigidBody;
        [HideInInspector]
        public RigidbodyConstraints2D defaultConstraints;

        PlayerStateMachine StateMachine;
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
            originalSpritesScale = playerSprites.transform.localScale;

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
            CheckInputs();
            AimReticule();
            StateMachine.OnUpdate(Time.deltaTime);
            currentStateName = StateMachine.GetCurrentStateName();
        }

        void FixedUpdate()
        {
            StateMachine.OnFixedUpdate();
            playerCollision.OnFixedUpdate();
        }

        private void CheckInputs()
        {
            move.x = RewiredPlayerInputManager.instance.GetHorizontalMovement();
            if (move.x != 0)
            {
                if (move.x > 0)
                {
                    playerSprites.transform.localScale = originalSpritesScale;
                    facingRight = true;
                }
                else
                {
                    playerSprites.transform.localScale = new Vector3(-originalSpritesScale.x, originalSpritesScale.y, originalSpritesScale.z);
                    facingRight = false;
                }
            }
        }

        private void AimReticule()
        {
            lookDirection = new Vector2(RewiredPlayerInputManager.instance.GetHorizontalMovement2(), RewiredPlayerInputManager.instance.GetVerticalMovement2());
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
        }
    }
}