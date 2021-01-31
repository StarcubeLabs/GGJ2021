using UnityEngine;

namespace GGJ2021.Enemy
{
    public class Goomba_Enemy : VulnerableEnemy_Enemy
    {
        [SerializeField]
        public static Sprite goombaSprite;

        public bool randomDirection;

        enum Direction { LEFT, RIGHT}

        [SerializeField]
        Direction myDir;

        //[SerializeField]
        float angleOfFoundation;

        /// <summary>
        /// Called by the NPCPool_Base to spawn an enemy.
        /// Receives its 'ticket' here
        /// </summary>
        /// <param name="v"></param>
        /// <param name="npcID"></param>
        public override void Spawn(Vector3 v, int npcID)
        {
            // Assign ticket
            NPCPoolID = npcID;

            // Handle missing or damaged gameobjects
            enemyName = "Goomba_" + npcID;
            this.gameObject.name = enemyName;
            if (myGraphicalParent == null)
            {
                myGraphicalParent = GetComponentInChildren<SpriteRenderer>().gameObject;

                if (myGraphicalParent == null)
                {
                    myGraphicalParent = new GameObject("GraphicalParent_Goomba" + npcID);
                    myGraphicalParent.transform.parent = this.transform;
                }
            }
            myGraphicalParent.name = "GraphicalParent_Goomba" + npcID;

            if (mySpriteRenderer == null)
            {
                mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();

                if (mySpriteRenderer == null)
                {

                    mySpriteRenderer = myGraphicalParent.AddComponent<SpriteRenderer>();
                    mySpriteRenderer.transform.parent = this.transform;

                    if (goombaSprite != null)
                        mySpriteRenderer.sprite = goombaSprite;
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogError("prefab lacked an assigned SpriteRenderer, and the Goomba enemy had no reference to a sprite to use. INVISIBLE: " + this.gameObject);
#endif
                    }
                }
            }

            // Init values
            Init(v);


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

        protected override void Init(Vector2 loc)
        {
            // Catchall inherited components
            base.Init(loc);

            EnemyState = ActorState.IDLE;

            restoreHealth();

            // Goomba Specific Components
            if (randomDirection)
            {
                int temp = Random.Range(0, 2);
                myDir = (Direction)temp;
            }


            // TO DO: set desired speed on Enemy_NPC.speed
        }

        /// <summary>
        /// Moves the Goomba left or right based on myDir
        /// Calls DetectEdge() to handle how it moves over the platforms
        /// </summary>
        protected override void Walk()
        {
            DetectEdge();

            if (!detect_Floor)
                myRigidBody.velocity = Vector3.down * 1f;
            else
            {
                switch (myDir)
                {
                    case Direction.LEFT:
                        myRigidBody.velocity = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector3.left) * speed * Time.deltaTime;
                        break;

                    case Direction.RIGHT:
                        myRigidBody.velocity = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector3.right) * speed * Time.deltaTime;
                        break;
                }
            }
        }

        //[SerializeField]
        // Fires straight forward at 'eye level'
        private bool detect_Wall;
        //[SerializeField]
        // Fires down and forward 
        private bool detect_WalkingCane;
        //[SerializeField]
        // Fires straight down
        private bool detect_Floor;

        // Down is relative to angleOfFoundation

        /// <summary>
        /// Detects whether or not the goomba will:
        /// 1) Fall: detectWall == false
        /// 2) Keep moving Right/Left
        /// 3) Turn Around: ledge found/wall found
        /// 4) Adjust its direction along the floor (handles slopes)
        /// </summary>
        void DetectEdge()
        {
            int wallCollisionDetection = 1 << 0; // Default Layer
            // Set Angle 
            Vector2 dir;
            Vector2 dir2;
            Vector2 dir3;
            RaycastHit2D r;

            // Falling
            if (!detect_Floor)
            {
                dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector2.down);
                r = Physics2D.Raycast(this.transform.position, dir, 0.5f, wallCollisionDetection);

                if (r)
                    detect_Floor = true;

            }
            else
            {

                // Detect Floor
                if (!detect_Wall)
                {
                    // If DetectWall is detected, then it could be a slope
                    dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector2.down);
                    detect_Floor = false;
                    r = Physics2D.Raycast(this.transform.position, dir, 1f, wallCollisionDetection);
                    dir3 = dir;
                    //Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + (dir3 * 1f), Color.red);
                    if (r)
                    {
                        detect_Floor = true;
                        // Adjust the angle of its right/left movement based on the angle of what it's standing on
                        angleOfFoundation = r.transform.rotation.eulerAngles.z;
                    }
                }

                // Walking Cane Check
                // Adjust Walking Cane Angle
                if (myDir == Direction.RIGHT)
                    dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * (Vector2.down + Vector2.right));
                else dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * (Vector2.down + Vector2.left));
                dir2 = dir;
                //Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + (dir2 * 5f), Color.green);
                detect_WalkingCane = false;
                r = Physics2D.Raycast(this.transform.position, dir, 5f, wallCollisionDetection);
                // Is it colliding
                if (r)
                {
                    detect_WalkingCane = true;
                    // Continue Forward
                }
                else if (myDir == Direction.LEFT) myDir = Direction.RIGHT; else myDir = Direction.LEFT;
                // else turn around

                // Detect Wall
                if (myDir == Direction.RIGHT)
                    dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * (Vector2.right));
                else dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * (Vector2.left));
                //Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + (dir * 0.5f), Color.blue);
                detect_Wall = false;
                r = Physics2D.Raycast(this.transform.position, dir, 0.5f, wallCollisionDetection);

                if (r)
                {
                    detect_Wall = true;
                    // Wall check
                    if (r.transform.rotation.z > 45)
                    {
                        // too steep, consider this a wall and not a slope
                        if (myDir == Direction.LEFT)
                            myDir = Direction.RIGHT;
                        else myDir = Direction.LEFT;
                    }
                    else angleOfFoundation = r.transform.rotation.eulerAngles.z;    // Set angle
                }
            }
        }

        void Update()
        {
            if (myDir == Direction.LEFT)
                facingRight = false;
            else facingRight = true;
        }


        void FixedUpdate()
        {
            if (!InheritedUpdateCall())
                return;

            switch (EnemyState)
            {
                case ActorState.PURSUIT:
                case ActorState.RETURNTOPOST:
                case ActorState.ATTACKING:
                case ActorState.IDLE:
                    Walk();
                    break;
            }
        }
    }
}
