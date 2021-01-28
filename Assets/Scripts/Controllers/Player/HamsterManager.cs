namespace GGJ2021
{
    using UnityEngine;

    public class HamsterManager : MonoBehaviour
    {
        public HamsterPool hamsterPool;

        private int curHamsterMax = 5;
        private int trueHamsterMax = 5;

        private float blasterCooldown, maxBlasterCooldown = 0.25f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShootProjectile(Vector2 origin, Vector2 velocity)
        {
            if (blasterCooldown <= 0f && GetAvailableHamsters() > 0)
            {
                hamsterPool.SpawnHamster(origin, velocity);
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
