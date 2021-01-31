namespace GGJ2021.Management
{
    using Audio;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneTransition : MonoBehaviour
    {
        [Tooltip("Unique ID of the door in the level.")]
        public int doorIndex;
        [Tooltip("Level scene to load.")]
        [SerializeField]
        private string levelToLoad;
        [Tooltip("Door index to spawn at in the next level.")]
        [SerializeField]
        private int levelToLoadDoorIndex;
        [SerializeField]
        private bool stopMusic;

        public Vector3 SpawnPoint
        {
            get { return transform.childCount == 0 ? transform.position : transform.GetChild(0).position; }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Transition();
        }

        public void Transition()
        {
            GameManager.Instance.CleanUp();
            LoadLevel();
        }

        public void LoadLevel()
        {
            if (stopMusic)
            {
                FmodFacade.instance.StopMusic();
            }
            PlayerStats.instance.spawnDoor = levelToLoadDoorIndex;
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
