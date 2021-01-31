namespace GGJ2021
{
    using UnityEngine;

    public class HamsterPoolable : MonoBehaviour
    {
        public int id;
        private float timer, maxTimer = 1f;
        private float explosionTimer, maxExplosionTimer = 0.25f;
        public bool canExplode = false;

        private Rigidbody2D rb;
        private Vector2 velocity;

        public SpriteRenderer hamster;

        public CollisionWrapper collisionWrapper;

        public HamsterExplosion hamsterExplosion;

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
            collisionWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);

            ResetHamster();
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Explode();
                }
            }
            if (explosionTimer > 0)
            {
                explosionTimer -= Time.deltaTime;
                if (explosionTimer <= 0)
                {
                    Disable();
                }
            }
        }

        void FixedUpdate()
        {
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            if (collisionWrapper.isActive)
            {
                rb.AddForce(PlayerController.instance.config.ProjectileGravity, ForceMode2D.Force);
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * -rb.velocity;
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
                transform.rotation = targetRotation;
            }
        }

        public void Launch(Vector2 origin, Vector2 velocity)
        {
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_shoot");
            ResetHamster();
            transform.position = origin;
            velocity.y += PlayerController.instance.config.ProjectileInitUpwardsBurst;
            rb.velocity = velocity;
            timer = maxTimer;
            canExplode = true;
        }

        public void ResetHamster()
        {
            collisionWrapper.SetActive(true);
            rb.velocity = Vector2.zero;
            hamster.enabled = true;
        }

        public void OnCollision(Collider2D collider)
        {
            Explode();
        }

        public void Explode()
        {
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("hamster_explosion_wet");
            canExplode = false;
            rb.velocity = Vector2.zero;
            collisionWrapper.SetActive(false);
            hamster.enabled = false;
            explosionTimer = maxExplosionTimer;
            Instantiate(hamsterExplosion, transform.position, Quaternion.identity);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}