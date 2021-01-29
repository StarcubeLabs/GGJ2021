<<<<<<< Updated upstream
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace GGJ2020.Enemy
{

    public class Enemy_Base : MonoBehaviour
    {
        public float maxhealth; // maxhealth is a float in case of peeshooter dmg
        public float health;    //current health of enemy
        public float damage;    //damage dealt to player
        public float moveSpeed; //move speed for mobile characters
        public float jellyLevel; 
        public float coolDown;
        public GameObject projectile;
        public List<GameObject> positions;
        public Collider2D hitbox;





        // Start is called before the first frame update
        void Start()
        {
            if(positions.Count == 0) // init to static lerping position
            {
                positions[0].transform.position = this.gameObject.transform.position;
                positions[1].transform.position = this.gameObject.transform.position;
            }      
        }

        // Update is called once per frame
        void Update()
        {

            this.gameObject.transform.position = Vector3.Lerp(positions[0].transform.position,   
                positions[1].transform.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f)); // Lerp between points

            if(health == 0)
            {
                Destroy(this.gameObject, 0.1f);
            }


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

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
        }
    }
}
=======
﻿namespace GGJ2021
{
    namespace Enemy
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

        public class Enemy_Base : MonoBehaviour
        {
            public float maxhealth;     //maxhealth is a float in case of peeshooter dmg
            public float health;        //current health of enemy
            public float damage;        //damage dealt to player
            public float moveSpeed;     //move speed for mobile characters
            public float coolDown;      //attack cooldown
            public GameObject projectile;
            public List<GameObject> positions; // init travel positions, static if left empty
            public Collider2D hitbox;
            private bool EnemyState;    // controls the enemey state




            // Start is called before the first frame update
            void Start()
            {
                if (positions.Count == 0) // init to static lerping position
                {
                    positions[0].transform.position = this.gameObject.transform.position;
                    positions[1].transform.position = this.gameObject.transform.position;
                }
            }

            // Update is called once per frame
            void Update()
            {
                this.gameObject.transform.position = Vector3.Lerp(positions[0].transform.position,
                    positions[1].transform.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f)); // Lerp between points

                if (health == 0)
                {
                    Destroy(this.gameObject, 0.1f);
                }

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

            public virtual void TakeDamage(float damage)
            {
                health -= damage;
            }
        }
    }
}
>>>>>>> Stashed changes
