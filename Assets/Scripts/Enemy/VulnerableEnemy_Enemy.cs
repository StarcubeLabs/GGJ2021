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
        public Collider2D myHurtBox;

        public virtual void hit()
        {
            // TO DO: Write default definition
        }

        public virtual void hit(int attackValue)
        {
            // TO DO: Write default definition
        }


        protected virtual void Init(Vector2 v)
        {
            mySpriteRenderer = myGraphicalParent.AddComponent<SpriteRenderer>();

            if(myHurtBox != null)
                myHurtBox.gameObject.layer = (int)collisionLayerIDs.ENEMY;

#if UNITY_EDITOR
            if (myHurtBox == null)
                Debug.LogError("Hurtbox not assigned to: " + enemyName);
#endif
        }
    }
}
