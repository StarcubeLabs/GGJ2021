using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public class EnemySpriteHolder_Base : MonoBehaviour
    {
        public bool PauseAll;
        bool paused;

        public Enemy_NPC[] prefabs;
        public GameObject spawn;
        NPCPool_Base x;

        private void Start()
        {
            Vector3 playerLoc = FindObjectOfType<PlayerController>().transform.position;
            NPCPool_Base.Instance.SpawnEnemy(spawn.transform.position, EnemyTypes.FLYER);
        }

        private void Update()
        {
            if (!paused)
            {
                if (PauseAll)
                {
                    NPCPool_Base.Instance.PauseAll();
                    paused = true;
                    PauseAll = false;
                }
            }else
            {
                if(PauseAll)
                {
                    NPCPool_Base.Instance.UnPauseAll();
                    paused = false;
                    PauseAll = false;
                }
            }
        }
    }
}
