namespace GGJ2021
{
    using UnityEngine;

    public class HamsterExplosion : MonoBehaviour
    {
        Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
            int direction = Random.Range(0, 1) * 2 - 1;
            float speed = Random.Range(PlayerController.instance.config.ProjectileRotationSpeedLowerBound,
                PlayerController.instance.config.ProjectileRotationSpeedUpperBound) * 0.5f;
            rb.angularVelocity = direction * speed;
            Destroy(gameObject, PlayerController.instance.config.ExplosionDuration);
        }
    }
}
