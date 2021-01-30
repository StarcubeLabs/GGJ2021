using UnityEngine;

namespace GGJ2021.Enemy
{

    public enum Hostility
    {
        ENEMY,
        FRIENDLY,
        NEUTRAL
    }

    public enum ActorState
    {
        PAUSED,
        IDLE,
        PURSUIT,
        ATTACKING,
        RETURNTOPOST
    }


    /// <summary>
    /// specifies the derived class must define hit
    /// </summary>
    interface IHittable
    {
        void hit();

        void hit(int attackValue);
    }

    public class NPC_Base :  FlippableCharacter
    {
        protected int NPCPoolID;
        protected int maxhealth;     //maxhealth is a float in case of peeshooter dmg
        protected int health;        //current health of enemy
        protected ActorState EnemyState;    // controls the enemey state

        protected enum collisionLayerIDs{Default, TransparentFX, IgnoreRaycast, THREE,Water, UI, SIX, SEVEN, PLAYER, ENEMY, PLAYERHITBOX, ENEMYHITBOX, PLAYERPROJECTILE, ENEMYPROJECTILE};

        protected SpriteRenderer mySpriteRenderer;
        protected GameObject myGraphicalParent;




        /// <summary>
        /// Called by the NPC_Pool Singleton Class when NPC_Pool is asked to pause all NPCs & bullets
        /// </summary>
        public void pause()
        {
            // TO DO: Write default definition
        }

        protected virtual void idle()
        {
        }
        protected virtual void patrol()
        {

        }
        protected virtual void combat()
        {

        }

        protected virtual void pursuit()
        {

        }
        protected virtual void returnToPost()
        {

        }

        /// <summary>
        /// Init calls this to handle set up that can happen at any time during runtime
        /// </summary>
        /// <param name="pos"></param>
        public virtual void Spawn(Vector3 v, int npcID)
        {
            // NPC_Pool.addToNPCPausePool();
        }

        protected virtual void Init(Vector2 v)
        {

        }

        /// <summary>
        /// Reinitialize location and default constructor values
        /// </summary>
        public virtual void respawn()
        {

        }
        protected void death()
        {
            // TO DO Write default definition
        }
    }
}

