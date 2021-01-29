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
        public Collider2D HurtBox;

        public virtual void hit()
        {
            // TO DO: Write default definition
        }

        public virtual void hit(int attackValue)
        {
            // TO DO: Write default definition
        }


        public override void spawn()
        {
#if UNITY_EDITOR
            if (HurtBox == null)
                Debug.LogError("Hurtbox not assigned to: " + this.gameObject);
#endif
        }
    }
}
