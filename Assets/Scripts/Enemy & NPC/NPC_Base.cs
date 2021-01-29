namespace GGJ2021
{
    namespace NPC
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        enum Hosility
        {
            ENEMY,
            FRIENDLY,
            NEUTRAL
        }

        enum ActorState
        {
            PAUSED,
            IDLE,
            PURSUIT,
            ATTACKING,
            RETURNTOPOST
        }

        /*
         * POOL STUFF:
         * NPC POOL
         * ENEMY AGROPOOL
         * BULLET POOL
         */
        public interface IHittable<T>
        {
            public virtual void hit()
            {

            }
            public virtual void hit(int attackValue)
            {

            }
        }
        public interface IContactDamager
        {
            public virtual void contact()
            {
            }
        }
        public interface IRangeAttacker
        {
            public virtual void aim();
            public virtual void fire();
        }
        public interface IMobileActor<T>
        {
            public virtual void walk(Vector3 pos1, Vector3 pos2);
            public virtual void fly(Vector3 pos1, Vector3 pos2);
            public virtual void jump(float jumpForce);
            public virtual void parabolicJump(Vector3 pos1, Vector3 pos2, Vector3 pos3);  //3 point arc
        }


        public class NPC_Base :  FlippableCharacter
        {
            public float maxhealth;     //maxhealth is a float in case of peeshooter dmg
            private float health;        //current health of enemy
            private bool EnemyState;    // controls the enemey state
            public float damage;        //damage dealt to player

            public float moveSpeed;
            public float coolDown;      //attack cooldown
            public GameObject projectile;
            public List<GameObject> positions; // init travel positions, static if left empty
            public Collider2D hitbox;

            private void OnApplicationPause(bool pause)
            {
                
            }
            public void idle()
            {

            }
            public void patrol()
            {

            }
            public void combat()
            {

            }
            public void returnToPost()
            {

            }
            public void spawn()
            {

            }
            public void respawn()
            {

            }
            public void death()
            {

            }
            // Start is called before the first frame update
            void Start()
            {
  
            }

            // Update is called once per frame
            void Update()
            {
    
            }

            private void OnCollisionEnter(Collision collision)
            {
                //add damage method on collision
            }

            public virtual void Move()
            {

            }

            public virtual void Attack()
            {

            }
        }
    }
}

