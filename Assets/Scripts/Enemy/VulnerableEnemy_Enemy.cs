using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public abstract class VulnerableEnemy_Enemy : Enemy_NPC, IHittable
    {
        /// <summary>
        /// Required 
        /// </summary>
        public CollisionWrapper hurtboxCollisionWrapper;

        public virtual void hit()
        {
            // TO DO: Write default definition

            healthChange();
        }

        public virtual void hit(int attackValue)
        {
            // TO DO: Write default definition
            if (attackValue > 0)
                attackValue = -attackValue;
            healthChange(attackValue);
        }


        protected virtual void Init(Vector2 v)
        {
            base.Init(v);

            mySpriteRenderer = myGraphicalParent.AddComponent<SpriteRenderer>();
            if (hurtboxCollisionWrapper != null)
            {
                hurtboxCollisionWrapper.AssignFunctionToTriggerEnterDelegate(OnCollision);
            }

#if UNITY_EDITOR
                if (hurtboxCollisionWrapper == null)
                Debug.LogError("HurtboxCollisionWrapper not assigned to: " + enemyName);
#endif
        }

        public void OnCollision(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox"))
            {
                hit();
            }
        }
    }
}
