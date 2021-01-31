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

        public override void Spawn(Vector3 v, int npcID)
        {
            NPCPoolID = npcID;
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
            Init(v);

            EnemyState = ActorState.IDLE;

            restoreHealth();
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

            // Goomba Specific Components
            if (randomDirection)
            {
                int temp = Random.Range(0, 2);
                myDir = (Direction)temp;
            }


            // TO DO: set desired speed on Enemy_NPC.speed
        }

        protected override void Walk()
        {
            // TO DO: 

            // myDir LEFT or RIGHT?
            // Raycast respectively using Vector2.Right/Left*speed, if not triggered
            // Overlap Circle with the centerpoint Vector2.Right*speed - Vector2.Up * 2, if triggered
            // Move respectively using transform+= Vector2.Right/Left * speed

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
        private bool detect_Wall;
        //[SerializeField]
        private bool detect_WalkingCane;
        //[SerializeField]
        private bool detect_Floor;

        void DetectEdge()
        {
            int wallCollisionDetection = 1 << 0; // Default Layer
            // Set Angle 
            Vector2 dir;
            Vector2 dir2;
            Vector2 dir3;
            RaycastHit2D r;

            if (!detect_Floor)
            {
                dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector2.down);
                r = Physics2D.Raycast(this.transform.position, dir, 0.5f, wallCollisionDetection);

                if (r)
                    detect_Floor = true;

            }
            else
            {

                // if detectWall whisker is not colliding
                if (!detect_Wall)
                {
                    dir = (Vector2)(Quaternion.Euler(0, 0, angleOfFoundation) * Vector2.down);
                    detect_Floor = false;
                    r = Physics2D.Raycast(this.transform.position, dir, 1f, wallCollisionDetection);
                    dir3 = dir;
                    //Debug.DrawLine(this.transform.position, (Vector2)this.transform.position + (dir3 * 1f), Color.red);
                    if (r)
                    {
                        detect_Floor = true;
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
