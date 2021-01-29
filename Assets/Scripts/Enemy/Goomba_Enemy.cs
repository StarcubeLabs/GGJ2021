using UnityEngine;

namespace GGJ2021.Enemy
{
    public class Goomba_Enemy : VulnerableEnemy_Enemy
    {
        enum Direction { LEFT, RIGHT}
        Direction myDir;

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


        public override void spawn()
        {
            base.spawn();
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
