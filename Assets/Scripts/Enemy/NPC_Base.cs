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
        [SerializeField]
        protected int maxhealth;     
        protected int health;        //current health of enemy
        protected ActorState EnemyState;    // controls the enemey state

        protected enum collisionLayerIDs{Default, TransparentFX, IgnoreRaycast, THREE,Water, UI, SIX, SEVEN, PLAYER, ENEMY, PLAYERHITBOX, ENEMYHITBOX, PLAYERPROJECTILE, ENEMYPROJECTILE};

        protected SpriteRenderer mySpriteRenderer;
        protected GameObject myGraphicalParent;
        public GameObject deathExplosion;



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
            NPCPoolID = npcID;
            Init(v);

            EnemyState = ActorState.IDLE;
        }

        protected virtual void Init(Vector2 v)
        {
            if (maxhealth == 0)
                maxhealth = 1;
        }

        /// <summary>
        /// Reinitialize location and default constructor values
        /// </summary>
        public virtual void respawn()
        {
            restoreHealth();
        }

        protected void restoreHealth()
        {
            health = maxhealth;
        }

        protected void death()
        {
            if (deathExplosion != null)
                Instantiate(deathExplosion, this.transform.position, Quaternion.identity);

            NPCPool_Base.Instance.RemoveFromNPCPool(NPCPoolID);

            Destroy(this.gameObject);
        }
    }
}

