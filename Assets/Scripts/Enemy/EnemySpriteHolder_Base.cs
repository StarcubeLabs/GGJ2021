using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021.Enemy
{
    public class EnemySpriteHolder_Base : MonoBehaviour
    {
        public Enemy_NPC[] prefabs;

        private void Start()
        {
            Vector3 playerLoc = FindObjectOfType<PlayerController>().transform.position;
            NPCPool_Base.Instance.SpawnEnemy(playerLoc, EnemyTypes.GOOMBA);
        }
    }
}
