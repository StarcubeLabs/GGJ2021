using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021.Enemy
{
    // This handles every enemy that can receive damage from the player
    public abstract class VulnerableEnemy_Enemy : Enemy_NPC, IHittable
    {
        /// <summary>
        /// Required 
        /// </summary>
        public CollisionWrapper hurtboxCollisionWrapper;

        /// <summary>
        /// Default damage to the enemy, always -1
        /// </summary>
        public virtual void hit()
        {
            healthChange();
        }

        /// <summary>
        /// Can specify damage to the enemy. Signed values matter.
        /// </summary>
        /// <param name="attackValue"></param>
        public virtual void hit(int attackValue)
        {
            if (attackValue > 0)
                attackValue = -attackValue;
            healthChange(attackValue);
        }

        /// <summary>
        /// Handles the HurtboxCollisionWrapper Assignment
        /// </summary>
        /// <param name="v"></param>
        protected virtual void Init(Vector2 v)
        {
            base.Init(v);

            if (myGraphicalParent == null)
                gameObject.GetComponentInChildren<SpriteRenderer>();

            if (mySpriteRenderer == null)
                gameObject.GetComponentInChildren<SpriteRenderer>();

            if(mySpriteRenderer == null)
            {
#if UNITY_EDITOR
                Debug.LogError("Prefab missing mySpriteRenderer assignment: " + enemyName);
#endif
            }

            if (hurtboxCollisionWrapper != null)
            {
                hurtboxCollisionWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);
            }

#if UNITY_EDITOR
                if (hurtboxCollisionWrapper == null)
                Debug.LogError("HurtboxCollisionWrapper not assigned to: " + enemyName);
#endif
        }

        /// <summary>
        /// Nick's Delegate for collision
        /// </summary>
        /// <param name="col"></param>
        public void OnCollision(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
            {
                hit();
            }
        }
    }
}
