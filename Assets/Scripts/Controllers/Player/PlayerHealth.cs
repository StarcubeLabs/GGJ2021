namespace GGJ2021
{
    using GGJ2021.UI;
    using UnityEngine;

    public class PlayerHealth
    {
        private int CurHealth
        {
            get { return PlayerStats.instance.CurHealth; }
            set
            {
                PlayerStats.instance.CurHealth = value;
                StatBarManager.instance.SetCurr(CurHealth);
            }
        }
        private int MaxHealth
        {
            get { return PlayerStats.instance.MaxHealth; }
            set { PlayerStats.instance.MaxHealth = value; }
        }
        private CollisionWrapper playerHurtboxColliderWrapper;

        private float invulnerabilityTimer, invulnerabilityTimerMax = 1f;
        private PlayerController controller;

        public bool dead = false;

        public PlayerHealth(PlayerController controller, CollisionWrapper playerHurtboxColliderWrapper)
        {
            // Initialize the HP bar at the start of the scene.
            StatBarManager.instance.SetCurr(CurHealth);
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
            if (invulnerabilityTimer > 0 || controller.playerPhysics.isDashing || controller.playerPhysics.isGrappling || dead || controller.goingThroughPipe)
            {
                return;
            }

            CurHealth -= damage;
            if (CurHealth <= 0)
            {
                Die();
            }
            else
            {
                FmodFacade.instance.CreateAndRunOneShotFmodEvent("player_damage");
                controller.playerAnimationController.HurtTrigger();
                invulnerabilityTimer = invulnerabilityTimerMax;
            }
        }

        public void RestoreHealth(int healing)
        {
            CurHealth += healing;
        }

        public void IncreaseMaxHealth(int amount)
        {
            MaxHealth += amount;
            CurHealth = MaxHealth;
        }

        private void Die()
        {
            FmodFacade.instance.CreateAndRunOneShotFmodEvent("player_death");
            controller.playerAnimationController.DeathTrigger();
            controller.cannon.SetActive(false);
            controller.StateMachine.ForceNextState(new PlayerStateDead(controller));
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
