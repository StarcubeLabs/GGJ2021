namespace GGJ2021
{
    using UnityEngine;

    public class HamsterManager : MonoBehaviour
    {
        public HamsterPool hamsterPool;

        private int curHamsterMax = 5;
        private int trueHamsterMax = 5;

        private float blasterCooldown = 0f;
        private float maxBlasterCooldown = 0f;

        void Update()
        {
            if (blasterCooldown > 0f)
            {
                blasterCooldown -= Time.deltaTime;
            }
        }

        public void ShootProjectile(Vector2 origin, Vector2 direction)
        {
            if (blasterCooldown <= 0f && GetAvailableHamsters() > 0)
            {
                blasterCooldown = maxBlasterCooldown;
                hamsterPool.SpawnHamster(origin, direction + PlayerController.instance.playerPhysics.GetVelocity());
            }
        }

        public int GetAvailableHamsters()
        {
            int availableHamsters = curHamsterMax;
            availableHamsters -= hamsterPool.GetActiveHamsters();
            //TODO subtract from available hamsters for using other abilities.
            return availableHamsters;
        }
    }
}
