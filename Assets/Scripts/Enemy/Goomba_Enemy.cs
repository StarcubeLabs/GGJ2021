using UnityEngine;

namespace GGJ2021.Enemy
{
    public class Goomba_Enemy : VulnerableEnemy_Enemy
    {
        [SerializeField]
        public static Sprite goombaSprite;

        enum Direction { LEFT, RIGHT}
        Direction myDir;

        public override void Spawn(Vector3 v, int npcID)
        {
            NPCPoolID = npcID;
            enemyName = "Goomba_" + npcID;
            this.gameObject.name = enemyName;

            if(myGraphicalParent == null)
                myGraphicalParent = new GameObject("GraphicalParent_Goomba" + npcID);
            myGraphicalParent.name = "GraphicalParent_Goomba" + npcID;

            if(mySpriteRenderer == null)
                mySpriteRenderer = myGraphicalParent.AddComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = goombaSprite;
            Init(v);

            EnemyState = ActorState.IDLE;
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
            myDir = (Direction)Random.Range(1, 2);

            // TO DO: set desired speed on Enemy_NPC.speed
        }

        protected override void walk(Vector3 pos1, Vector3 pos2)
        {
            // TO DO: 

            // myDir LEFT or RIGHT?
                // Raycast respectively using Vector2.Right/Left*speed, if not triggered
                    // Overlap Circle with the centerpoint Vector2.Right*speed - Vector2.Up * 2, if triggered
                        // Move respectively using transform+= Vector2.Right/Left * speed
        }


        void FixedUpdate()
        {
            switch(EnemyState)
            {
                case ActorState.IDLE:

                    switch (myDir)
                    {
                        case Direction.LEFT:
                            this.transform.position += Vector3.left * speed * Time.time;
                            break;

                        case Direction.RIGHT:
                            this.transform.position += Vector3.right * speed * Time.time;
                            break;
                    }
                    break;

                case ActorState.PURSUIT:
                    break;

                case ActorState.RETURNTOPOST:
                    break;

                case ActorState.ATTACKING:
                    break;
            }
        }
    }
}
