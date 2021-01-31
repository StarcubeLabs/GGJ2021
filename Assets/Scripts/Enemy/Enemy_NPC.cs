using UnityEngine;

namespace GGJ2021.Enemy
{
    public abstract class Enemy_NPC : NPC_Base
    {
        // Hold the index of the NPC_Pool.EnemyPool. 
        // -1 when enemy is not aggro
        protected int flockID;        
        protected string enemyName;
        protected Rigidbody2D myRigidBody;

        [SerializeField]
        protected float speed;

        #region Shooting Enemy functions
        // empty virtuals
        protected virtual void fire()
        {
            // TO DO: Write default definition here
        }

        protected virtual void aim()
        {
            // TO DO: Write default definition here
        }
        #endregion


        #region Default Movement Definitions
        // Empty virtuals
        protected virtual void Walk()
        {
            // TO DO: Write default definition
        }
        protected virtual void fly(Vector3 pos1, Vector3 pos2)
        {
            // TO DO: Write default definition
        }
        protected virtual void jump(float jumpForce)
        {
            // TO DO: Write default definition
        }
        protected virtual void parabolicJump(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            // TO DO: Write default definition
        }
        #endregion

        #region HealthChange
        /// <summary>
        /// Hurts the enemy for 1 damage
        /// </summary>
        public virtual void healthChange()
        {
            health--;

            if (health > maxhealth)
                health = maxhealth;

            if (health <= 0)
            {
                death();
            }
        }

        /// <summary>
        /// Changes the health by the specified amount SIGNED NUMBERS MATTER
        /// </summary>
        /// <param name="value"></param>
        public void healthChange(int value)
        {
            health += value;

            if (health > maxhealth)
                health = maxhealth;

            if(health <= 0)
            {
                death();
            }
        }
        #endregion


        /// <summary>
        /// Handles Rigidbody assignment
        /// </summary>
        /// <param name="v"></param>
        protected virtual void Init(Vector2 v)
        {
            base.Init(v);

            if (myRigidBody == null)
            {
                myRigidBody = GetComponent<Rigidbody2D>();

                if (myRigidBody == null)
                    myRigidBody = gameObject.AddComponent<Rigidbody2D>();
            }
        }
    }
}
