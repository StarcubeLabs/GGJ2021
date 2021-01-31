namespace GGJ2021.Management
{
    using Audio;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneTransition : MonoBehaviour
    {
        public string LevelToLoad;
        public bool StopMusic;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        public void MenuLoad(string level)
        {
            SceneManager.LoadScene(level);
        }

        public void Transition()
        {
            GameManager.Instance.CleanUp();
            FmodFacade.instance.StopMusic();
            SceneManager.LoadScene(LevelToLoad);
        }
    }
}
