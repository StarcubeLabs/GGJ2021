using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public class Wasp_Enemy : VulnerableEnemy_Enemy
    {
        [SerializeField]
        public static Sprite WaspSprite;

        private PlayerController playerRef;

        [SerializeField]
        float idleTimer = 0;
        public float AggroRange;

        public override void Spawn(Vector3 v, int npcID)
        {
            NPCPoolID = npcID;

            enemyName = "Wasp_" + npcID;

            this.gameObject.name = enemyName;

            if(myGraphicalParent == null)
            {
                myGraphicalParent = GetComponentInChildren<SpriteRenderer>().gameObject;
                if(myGraphicalParent == null)
                {
                    myGraphicalParent = new GameObject("GraphicalParent_Wasp" + npcID);
                    myGraphicalParent.transform.parent = this.transform;
                }
            }
            myGraphicalParent.name = "GraphicalParent_Wasp" + npcID;

            if(mySpriteRenderer == null)
            {
                mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
                if(mySpriteRenderer == null)
                {
                    mySpriteRenderer = myGraphicalParent.AddComponent<SpriteRenderer>();
                    mySpriteRenderer.transform.parent = this.transform;

                    if (WaspSprite != null)
                        mySpriteRenderer.sprite = WaspSprite;
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogError("prefab lacked an assigned SpriteRenderer, and the Wasp enemy had no reference to a sprite to use. INVISIBLE: " + this.gameObject);
#endif
                    }
                }
            }
            Init(v);

            EnemyState = ActorState.IDLE;

            restoreHealth();
        }

        protected override void Init(Vector2 v)
        {
            base.Init(v);

            playerRef = PlayerController.instance;
            if (AggroRange <= 0)
                AggroRange = 10.0f;

            findNewAngle = true;
        }

        #region Hit variants
        public override void hit()
        {
            base.hit();
        }

        public override void hit(int attackValue)
        {
            base.hit(attackValue);
        }
        #endregion

        Vector2 movementDirectionVec;
        Vector2 prevMovementDirectionVec;
        [SerializeField]
        private float movementDirection = 0;
        [SerializeField]
        bool findNewAngle;

        private void DetectPlayer()
        {
            Vector2 playerPos = playerRef.transform.position;

            if(Vector2.Distance(this.transform.position, playerPos) < AggroRange)
            {
                int player_LayerMask = 1 << (int)collisionLayerIDs.PLAYER;
                RaycastHit2D r = Physics2D.Raycast(this.transform.position, playerPos - (Vector2)this.transform.position, AggroRange, player_LayerMask);
                if(r)
                {
                    EnemyState = ActorState.PURSUIT;
                    findNewAngle = true;
                    idleTimer = 0;
                }
            }
        }

        float dotProduct(Vector2 a, Vector2 b)
        {
            float ret = (a.x * b.x + a.y * b.y);
            debugVector = ret;
            return ret;
        }

        Vector2 attackVector;
        [SerializeField]
        float debugVector;

        void Attack()
        {
            Vector2 temp = (playerRef.transform.position - this.transform.position).normalized;
            if(findNewAngle)
            {
                attackVector = temp;
                findNewAngle = false;
                idleTimer = 0;
            }

            // Slowing Down
            if(idleTimer > 0)
            {
                myRigidBody.velocity = myRigidBody.velocity / 1.7f;
                idleTimer += Time.deltaTime;

                // Change to new 
                if (idleTimer > 3.0f)
                {
                    idleTimer = 0;
                    findNewAngle = true;

                    myRigidBody.velocity = Vector2.zero;
                    EnemyState = ActorState.RETURNTOPOST;
                }
            }else
            {
                // Begin Deceleration having passed the player
                if (dotProduct(attackVector, temp) < 0)
                {
                    myRigidBody.velocity = myRigidBody.velocity / 1.7f;
                    idleTimer += Time.deltaTime;
                }else
                {
                    // Accelerating into the player
                    myRigidBody.velocity += attackVector * speed /5* Time.deltaTime;

                    if (myRigidBody.velocity.magnitude > 500f)
                        myRigidBody.velocity = myRigidBody.velocity.normalized *500f;

                    // Will stop short if it is about to hit a wall
                    int wallLayerMask = 1 << (int)collisionLayerIDs.Default;
                    Collider2D r = Physics2D.OverlapCircle((Vector2)this.transform.position + myRigidBody.velocity*(Time.deltaTime * 1), 1f, wallLayerMask);
                    //Debug.DrawLine(this.transform.position, this.transform.position+ (Vector3)myRigidBody.velocity*Time.deltaTime, Color.red);
                    if (r != null)
                    {
                        RaycastHit2D rx = Physics2D.Raycast((Vector2)this.transform.position + attackVector.normalized * 1f, attackVector.normalized, 5f, wallLayerMask);
                        if (rx)
                        {
                            if (rx.distance <= 1)
                            {
                                EnemyState = ActorState.IDLE;
                                findNewAngle = true;
                                idleTimer = 0;
                            }
                        }
                    }
                }
            }
        }


        Vector2 closetContactPoint;
        protected override void Walk()
        {
            int layerMask = 1 << (int)collisionLayerIDs.Default;
            if(findNewAngle)
            {
                if (closetContactPoint == Vector2.zero)
                    prevMovementDirectionVec = (Vector2)this.transform.position - (Vector2)playerRef.transform.position;
                else prevMovementDirectionVec = closetContactPoint - (Vector2)this.transform.position;

                do
                {
                    movementDirection = Random.Range(0, 361);

                    movementDirectionVec = (Vector2)(Quaternion.Euler(0, 0, movementDirection) * Vector2.right) * speed * Time.deltaTime;
                } while (dotProduct(movementDirectionVec.normalized, prevMovementDirectionVec.normalized) > 0.5f);

                RaycastHit2D r1;// = Physics2D.Raycast(this.transform.position, movementDirectionVec, 5f, layerMask);
                Vector2 temp;
                float maxDist = -1;
                Vector2 maxTemp = Vector2.zero;
                float maxAngleTemp = 0;
                for(int i = 0; i < 361; i++)
                {
                    temp = (Vector2)(Quaternion.Euler(0, 0, i) * movementDirectionVec.normalized);

                    r1 = Physics2D.Raycast(this.transform.position, temp.normalized, 10f, layerMask);

                    if (!r1)
                    {
                        movementDirectionVec = temp;
                        movementDirection += i;
                    }

                    if (maxDist < 0 || r1.distance > maxDist)
                    {
                        maxDist = r1.distance;
                        maxTemp = temp;
                        maxAngleTemp = i;
                    }

                    if (i == 360)
                    {
                        movementDirectionVec = maxTemp;
                        movementDirection += maxAngleTemp;
                    }
                }

                //Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + movementDirectionVec.normalized * 10f, Color.red);

                findNewAngle = false;
            }

            myRigidBody.velocity = movementDirectionVec.normalized * speed * Time.deltaTime;

            Collider2D r = Physics2D.OverlapCircle((Vector2)this.transform.position + myRigidBody.velocity * Time.deltaTime, 1.5f, layerMask);
            if(r != null)
            {
                closetContactPoint = Physics2D.ClosestPoint(this.transform.position, r);
                RaycastHit2D rx = Physics2D.Raycast((Vector2)this.transform.position+ movementDirectionVec.normalized * 1f, movementDirectionVec.normalized, 5f, layerMask);

                //Debug.DrawLine((Vector2)this.transform.position + movementDirectionVec.normalized * 1f, (Vector2)this.transform.position + movementDirectionVec.normalized * 5f, Color.green);
                
                if(rx)
                {

                    if(rx.distance <= 1.5f)
                    {
                        EnemyState = ActorState.IDLE;
                        findNewAngle = true;
                        idleTimer = 0;

                        myRigidBody.velocity = Vector2.zero;
                    }

                }

            }
            
        }

        private void FixedUpdate()
        {
            if (!InheritedUpdateCall())
                return;

            switch(EnemyState)
            {
                case ActorState.IDLE:
                    myRigidBody.velocity = Vector2.zero;
                    break;
                case ActorState.PURSUIT:
                    myRigidBody.velocity = Vector2.zero;
                    break;
                case ActorState.ATTACKING:
                    Attack();
                    break;
                case ActorState.RETURNTOPOST:
                    Walk();
                    break;
            }
        }

        private void Update()
        {
            if (!InheritedUpdateCall())
                return;

            if(EnemyState != ActorState.PURSUIT && EnemyState != ActorState.ATTACKING)
            {
                DetectPlayer();
            }

            switch (EnemyState)
            {
                case ActorState.IDLE:
                    if (idleTimer >= 3.5)
                    {
                        idleTimer = 0;
                        EnemyState = ActorState.RETURNTOPOST;
                    }else idleTimer += Time.fixedDeltaTime;
                    break;
                case ActorState.PURSUIT:
                    if (idleTimer >= 1)
                    {
                        idleTimer = 0;
                        EnemyState = ActorState.ATTACKING;
                    }
                    else idleTimer += Time.fixedDeltaTime;
                    break;
                case ActorState.ATTACKING:
                case ActorState.RETURNTOPOST:
                    break;
            }
        }
    }
}
