namespace GGJ2021.Managment
{
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Assertions;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameObject[] PersistentObjects;
        public GameObject[] SceneLocalObjects;

        private GameObject[] persistent;
        private GameObject[] local;

        private void Start()
        {
            foreach (GameObject p in PersistentObjects)
                Assert.IsNotNull(p);

            foreach (GameObject l in SceneLocalObjects)
                Assert.IsNotNull(l);

            if (Instance == null)
                Init();
            else
                Destroy(this.gameObject);
        }

        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            persistent = new GameObject[PersistentObjects.Length];
            local = new GameObject[SceneLocalObjects.Length];
            for(int i = 0; i < persistent.Length; i++)
            {
                persistent[i] = Instantiate(PersistentObjects[i]);
                DontDestroyOnLoad(persistent[i]);
            }

            ReInit();
        }

        public void ReInit()
        {
            if (local.Any(x => x == null))
                CleanUp();

            for (int i = 0; i < local.Length; i++)
                local[i] = Instantiate(SceneLocalObjects[i]);
        }

        public void CleanUp()
        {
            for (int i = 0; i < local.Length; i++)
            {
                if (local[i] != null)
                {
                    Destroy(local[i]);

                    // Ensure reference is nulled immediately to prevent access after free issues
                    local[i] = null;
                }
            }
        }

        public void ForceClean()
        {
            CleanUp();
            for (int i = 0; i < persistent.Length; i++)
            {
                if (persistent[i] != null)
                {
                    Destroy(persistent[i]);

                    // Ensure reference is nulled immediately to prevent access after free issues
                    persistent[i] = null;
                }
            }

            Instance = null;
            Destroy(this.gameObject);
        }
    }
}
