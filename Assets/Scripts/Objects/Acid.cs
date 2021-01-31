namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// Deals damage and sends the player back to their spawn point.
    /// </summary>
    class Acid : MonoBehaviour
    {
        [Tooltip("Amount of damage taken from touching acid.")]
        [SerializeField]
        private int damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerPhysics"))
            {
                PlayerController.instance.playerHealth.TakeDamage(damage);
                if (!PlayerController.instance.playerHealth.dead)
                {
                    // Sanity check to respawn somewhere even if spawn door isn't set.
                    PlayerStats.instance.spawnDoor = Mathf.Max(0, PlayerStats.instance.spawnDoor);
                    //TODO Play animation.
                    PlayerController.instance.Spawn();
                }
            }
        }
    }
}