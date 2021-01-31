namespace GGJ2021.Management
{
    using UnityEngine;
    using Enemy;

    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;

        public enum EnemyType { Goomba, Wasp, Turret }

        public GameObject[] EnemyPrefabs;

        public GameObject[] SpawnedEnemies;

        private void Start()
        {
            Instance = this;
            EnemySpawnData data = FindObjectOfType<EnemySpawnData>();
            if (data != null)
            {
                SpawnedEnemies = new GameObject[data.SpawnPoints.Length];
                for (int i = 0; i < SpawnedEnemies.Length; i++)
                {
                    SpawnedEnemies[i] = Instantiate(EnemyPrefabs[(int)data.Types[i]]);
                    SpawnedEnemies[i].GetComponent<NPC_Base>().Spawn(data.SpawnPoints[i].position, 0);
                    SpawnedEnemies[i].transform.position = data.SpawnPoints[i].position;
                }
            }
        }

        public void CleanUp()
        {
            Instance = null;
            if (SpawnedEnemies != null)
            {
                foreach (GameObject g in SpawnedEnemies)
                {
                    if (g != null)
                        Destroy(g);
                }
            }
        }
    }
}
