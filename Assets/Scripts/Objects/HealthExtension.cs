namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// Collectible that increases the player's max health.
    /// </summary>
    public class HealthExtension : Collectible
    {
        [Tooltip("Amount to increase health by.")]
        [SerializeField]
        private int healthIncreaseAmount;

        protected override void OnCollect()
        {
            PlayerController.instance.playerHealth.IncreaseMaxHealth(healthIncreaseAmount);
        }
    }
}
