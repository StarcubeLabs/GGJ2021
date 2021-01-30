namespace GGJ2021
{
    using GGJ2021.UI;
    using UnityEngine;

    public class PlayerHealth
    {
        private int curHealth, maxHealth = 3;
        private CollisionWrapper playerHurtboxColliderWrapper;

        public PlayerHealth(PlayerController controller, CollisionWrapper playerHurtboxColliderWrapper)
        {
            curHealth = maxHealth;
            this.playerHurtboxColliderWrapper = playerHurtboxColliderWrapper;
            this.playerHurtboxColliderWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);
        }

        public void TakeDamage(int damage)
        {
            curHealth -= damage;
            StatBarManager.instance.SetCurr(curHealth);
            if (curHealth <= 0)
            {
                Die();
            }
        }

        public void RestoreHealth(int healing)
        {
            curHealth = Mathf.Min(maxHealth, curHealth + healing);
            StatBarManager.instance.SetCurr(curHealth);
        }

        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
            curHealth = maxHealth;
            StatBarManager.instance.SetMax(maxHealth);
        }

        private void Die()
        {
            //TODO: Die.
        }

        public void OnCollision(Collider2D col)
        {
            TakeDamage(1);
        }
    }
}
