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
        }

        // Update is called once per frame
        void Update()
        {
            move.x = RewiredPlayerInputManager.instance.GetHorizontalMovement();
            if (move.magnitude > 0)
            {
                rb.velocity = move.normalized * movementSpeed;
            }
        }
    }
}