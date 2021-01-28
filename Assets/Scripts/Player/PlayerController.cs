namespace Starjam
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        public float movementSpeed = 5f;
        private Vector2 move = new Vector2();
        private Vector2 lookDirection = new Vector2();
        private float reticuleDistance = 0.3f;
        private bool facingRight = true;

        public GameObject playerSprites;
        private Vector3 originalSpritesScale;
        public GameObject reticule;

        private Rigidbody2D rb;

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
            rb = GetComponentInChildren<Rigidbody2D>();
            originalSpritesScale = playerSprites.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            AimReticule();
        }

        private void Move()
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
                rb.velocity = new Vector2(move.x * movementSpeed, rb.velocity.y);
            }
        }

        private void AimReticule()
        {
            lookDirection = new Vector2(RewiredPlayerInputManager.instance.GetHorizontalMovement2(), RewiredPlayerInputManager.instance.GetVerticalMovement2());
            if (lookDirection != Vector2.zero)
            {
                lookDirection = lookDirection.normalized * reticuleDistance;
            }
            else
            {
                if (facingRight)
                {
                    lookDirection = new Vector2(reticuleDistance, 0f);
                }
                else
                {
                    lookDirection = new Vector2(-reticuleDistance, 0f);
                }
            }
            reticule.transform.localPosition = lookDirection;
        }
    }
}