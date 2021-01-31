namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// Collectible that heals the player.
    /// </summary>
    public class HealthRestore : Collectible
    {
        [Tooltip("Amount to heal by.")]
        [SerializeField]
        private int healthHealAmount;

        protected override void OnCollect()
        {
            PlayerController.instance.playerHealth.RestoreHealth(healthHealAmount);
        }
    }
}
