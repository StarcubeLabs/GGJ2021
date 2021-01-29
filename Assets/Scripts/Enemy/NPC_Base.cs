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
        protected int maxhealth;     //maxhealth is a float in case of peeshooter dmg
        protected int health;        //current health of enemy
        protected ActorState EnemyState;    // controls the enemey state

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
        public virtual void spawn()
        {
            // NPC_Pool.addToNPCPausePool();
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

