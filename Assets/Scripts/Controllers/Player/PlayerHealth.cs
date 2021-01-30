namespace GGJ2021
{
    using GGJ2021.UI;
    using UnityEngine;

    public class PlayerHealth
    {
        private int curHealth, maxHealth = 3;
        private CollisionWrapper playerHurtboxColliderWrapper;

        private float invulnerabilityTimer, invulnerabilityTimerMax = 1f;
        private PlayerController controller;

        public bool dead = false;

        public PlayerHealth(PlayerController controller, CollisionWrapper playerHurtboxColliderWrapper)
        {
            curHealth = maxHealth;
            this.controller = controller;
            this.playerHurtboxColliderWrapper = playerHurtboxColliderWrapper;
            this.playerHurtboxColliderWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);
        }

        public void OnUpdate(float deltaTime)
        {
            if (invulnerabilityTimer > 0)
            {
                invulnerabilityTimer -= deltaTime;
            }
        }

        public void TakeDamage(int damage)
        {
            if (invulnerabilityTimer > 0)
            {
                return;
            }

            curHealth -= damage;
            StatBarManager.instance.SetCurr(curHealth);
            if (curHealth <= 0)
            {
                Die();
            }
            else
            {
                controller.playerAnimationController.HurtTrigger();
                invulnerabilityTimer = invulnerabilityTimerMax;
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
            controller.playerAnimationController.DeathTrigger();
            dead = true;
        }

        public void OnCollision(Collider2D col)
        {
            TakeDamage(1);
        }

        public bool IsInvulnerableFromHit()
        {
            return invulnerabilityTimer > 0;
        }
    }
}
