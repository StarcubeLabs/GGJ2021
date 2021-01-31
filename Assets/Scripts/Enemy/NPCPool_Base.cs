using UnityEditor;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public enum EnemyTypes { GOOMBA, TURRET, FLYER}
    public sealed class NPCPool_Base : MonoBehaviour
    {

        private static NPCPool_Base instance;
        private static object padlock = new object();
        public EnemySpriteHolder_Base enemyPrefabLibrary;


        NPCPool_Base()
        {
            npcPausePool = new NPC_Base[100];
            aggroPool = new Enemy_NPC[100];
        }

        public static NPCPool_Base Instance
        {
            get { if (instance == null) { GameObject g = new GameObject();  instance = g.AddComponent<NPCPool_Base>(); instance.enemyPrefabLibrary = FindObjectOfType<EnemySpriteHolder_Base>();
#if UNITY_EDITOR
                    if (instance.enemyPrefabLibrary == null)
                        Debug.LogError("Assets/Prefabs/EnemySpritePrefabs missing from scene. CANNOT SPAWN ENEMIES");
#endif
                    Debug.LogError("make new singleton: " + instance); } return instance; }
        }

        NPC_Base[] npcPausePool;
        static int npcPausePoolCount;
        Enemy_NPC[] aggroPool;
        static int aggroPoolCount;

        // BulletPool[]

        public void SpawnEnemy(Vector3 v, EnemyTypes eT)
        {
            if (enemyPrefabLibrary.prefabs.Length > 0)
            {
                Enemy_NPC tempEnemy;
                int tempID = AddToNPCPool();

                if (tempID < 0)
                {
                    Debug.LogError("Error in assigning NPC_ID");
                    return;
                }

                switch (eT)
                {
                    case EnemyTypes.GOOMBA:

                        if (enemyPrefabLibrary.prefabs.Length <= (int)eT)
                        {
                            tempEnemy = Instantiate<Enemy_NPC>(enemyPrefabLibrary.prefabs[0]);
                            AddToPool<NPC_Base>(instance.npcPausePool, tempEnemy, tempID);
                            tempEnemy.Spawn(v, tempID);
                        }else
                        {
                            tempEnemy = Instantiate<Enemy_NPC>(enemyPrefabLibrary.prefabs[(int)eT]);
                            AddToPool<NPC_Base>(instance.npcPausePool, tempEnemy, tempID);

                            tempEnemy.Spawn(v, tempID);
                        }

                        break;

                    case EnemyTypes.TURRET:
                        //tempEnemy.NPCID = AddToNPCPool(tempEnemy);

                        break;

                    case EnemyTypes.FLYER:
                        //tempEnemy.NPCID = AddToNPCPool(tempEnemy);

                        break;
                }
#if UNITY_EDITOR
                if (enemyPrefabLibrary.prefabs.Length <= (int)eT)
                    Debug.LogError("current Assets/Prefabs/EnemySpritePrefabs does not contain desired sprite: " + eT + ". Defaulting to Goomba Prefab");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Assets/Prefabs/EnemySpritePrefabs contains no prefab in this scene");
#endif
            }
        }

        public void PauseAll()
        {
            int total = 0;
            for (int i = 0; i < instance.npcPausePool.Length; i++)
            {
                if (instance.npcPausePool[i] != null)
                {
                    total++;
                    instance.npcPausePool[i].pause();
                }

                if (total >= npcPausePoolCount)
                    break;
            }

            // TO DO: Pause Bullets 
        }

        public void UnPauseAll()
        {
            int total = 0;
            for (int i = 0; i < instance.npcPausePool.Length; i++)
            {
                if (instance.npcPausePool[i] != null)
                {
                    total++;

                    instance.npcPausePool[i].Unpause();
                }

                if (total >= npcPausePoolCount)
                    break;
            }

            // TO DO: Unpause Bullets 
        }

        private int AddToNPCPool()
        {
            int i = 0;
            bool emptyspotNotFound = true;
            while (emptyspotNotFound && i < 100)
            {
                if (instance.npcPausePool[i] == null)
                {
                    npcPausePoolCount++;

                    emptyspotNotFound = false;
                }
                else i++;
            }

            if (i >= 100)
                return -1;
            else return i;
        }

        public int AddToAggroPool(Enemy_NPC eNPC)
        {
            int i = 0;
            bool emptyspotNotFound = true;
            while (emptyspotNotFound && i < 100)
            {
                if (instance.aggroPool[i] == null)
                {
                    instance.aggroPool[i] = eNPC;
                    aggroPoolCount++;

                    emptyspotNotFound = false;
                }
                else i++;
            }

            if (i >= 100)
                return -1;
            else return i;
        }

        public void RemoveFromNPCPool(int flockID)
        {
            RemoveFromPool<NPC_Base>(instance.npcPausePool, flockID, npcPausePoolCount);
        }

        public void RemoveFromAggroPool(int flockID)
        {
            RemoveFromPool<Enemy_NPC>(instance.aggroPool, flockID, aggroPoolCount);
        }

        private void RemoveFromPool<T>(T[] pool, int flockNum, int poolSize)
        {
            int j = 0;
            int j_countingValidUnits = 0;
            while (j_countingValidUnits < poolSize && j < 100)
            {
                if (pool[j] != null)
                {
                    j_countingValidUnits++;
                }
                j++;
            }
            pool[flockNum] = default(T);
            poolSize--;
        }

        private static void AddToPool<T>(T[] pool, T member, int index)
        {
            pool[index] = member;
        }
    }
}
