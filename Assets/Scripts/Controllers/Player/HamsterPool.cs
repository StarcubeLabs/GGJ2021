namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HamsterPool : MonoBehaviour
    {
        private List<HamsterPoolable> hamsters;
        public int poolSize = 5;

        public HamsterPoolable hamsterPoolablePrefab;

        // Start is called before the first frame update
        void Start()
        {
            hamsters = new List<HamsterPoolable>();
            for (int i = 0; i < poolSize; i++)
            {
                hamsters.Add(Instantiate(hamsterPoolablePrefab, transform));
                hamsters[i].id = i;
                hamsters[i].gameObject.SetActive(false);
            }
        }

        public bool SpawnHamster(Vector2 origin, Vector2 velocity)
        {
            foreach (HamsterPoolable hamster in hamsters)
            {
                if (hamster.gameObject.activeSelf == false)
                {
                    hamster.gameObject.SetActive(true);
                    hamster.Launch(origin, velocity);
                    return true;
                }
            }
            return false;
        }

        public int GetActiveHamsters()
        {
            int activeHamsters = 0;
            foreach (HamsterPoolable hamster in hamsters)
            {
                if (hamster.gameObject.activeSelf == true)
                {
                    activeHamsters++;
                }
            }
            return activeHamsters;
        }
    }
}
