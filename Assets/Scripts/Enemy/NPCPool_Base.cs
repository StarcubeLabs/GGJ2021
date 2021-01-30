using UnityEditor;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public enum EnemyTypes { GOOMBA, TURRET, FLYER}
    public sealed class NPCPool_Base : MonoBehaviour
    {

        private static NPCPool_Base instance = null;
        private static object padlock = new object();
        public EnemySpriteHolder_Base enemyPrefabLibrary;

        NPCPool_Base()
        {
            npcPausePool = new NPCPool_Base[100];
            aggroPool = new Enemy_NPC[100];
            enemyPrefabLibrary = FindObjectOfType<EnemySpriteHolder_Base>();

#if UNITY_EDITOR
            if (enemyPrefabLibrary == null)
                Debug.LogError("Assets/Prefabs/EnemySpritePrefabs missing from scene. CANNOT SPAWN ENEMIES");
#endif
        }

        public static NPCPool_Base Instance
        {
            get { lock (padlock) { if (instance == null) { instance = new NPCPool_Base(); } return instance; } }
        }

        private NPCPool_Base[] npcPausePool;
        private Enemy_NPC[] aggroPool;

        // BulletPool[]

        public void SpawnEnemy(Vector3 v, EnemyTypes eT)
        {
            if (enemyPrefabLibrary.prefabs.Length > 0)
            {
                Enemy_NPC tempEnemy;
                int tempID = AddToNPCPool();

                if (tempID < 0)
                {
                    Debug.Log("Error in assigning NPC_ID");
                    return;
                }

                switch (eT)
                {
                    case EnemyTypes.GOOMBA:

                        if (enemyPrefabLibrary.prefabs.Length <= (int)eT)
                        {
                            tempEnemy = Instantiate<Enemy_NPC>(enemyPrefabLibrary.prefabs[(int)eT]);
                            tempEnemy.Spawn(v, tempID);
                        }else
                        {
                            tempEnemy = Instantiate<Enemy_NPC>(enemyPrefabLibrary.prefabs[0]);
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

        private int AddToNPCPool()
        {
            int ret = 0;
            // TO DO: Search pool for a free space and then assign that value to the NPC

           
            return ret;
        }
    }
}
