namespace GGJ2021.Management
{
    using Audio;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Assertions;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool StartingInMenu;
        public string Song;
        public float Volume;
        public FmodFacade FModPrefab;

        public GameObject[] PersistentObjects;

        public PlayerController PlayerManagerPrefab;
        public GameObject[] SceneLocalObjects;

        private FmodFacade fmod;
        private string currentSong;
        private GameObject[] persistent;

        private GameObject player;
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
            {
                if (!StartingInMenu)
                    Instance.ReInit();

                Instance.StartSong(Song, Volume);
                Destroy(this.gameObject);
            }
        }

        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            persistent = new GameObject[PersistentObjects.Length];
            local = new GameObject[SceneLocalObjects.Length];

            fmod = Instantiate(FModPrefab.gameObject).GetComponent<FmodFacade>();
            DontDestroyOnLoad(fmod.gameObject);
            StartSong(Song, Volume);

            VolatileInit();
            if (!StartingInMenu)
            {
                ReInit();
            }
        }

        public void StartSong(string song, float volume)
        {
            if (song == currentSong)
                return;

            fmod.StartMusic(song, volume);
            currentSong = song;
        }

        public void VolatileInit()
        {
            if (persistent.Any(x => x != null))
                return;

            for (int i = 0; i < persistent.Length; i++)
            {
                persistent[i] = Instantiate(PersistentObjects[i]);
                DontDestroyOnLoad(persistent[i]);
            }
        }

        public void ReInit()
        {
            if (player != null || local.Any(x => x == null))
                CleanUp();

            player = Instantiate(PlayerManagerPrefab.gameObject);
            for (int i = 0; i < local.Length; i++)
                local[i] = Instantiate(SceneLocalObjects[i]);
        }

        public void CleanUp()
        {
            Destroy(player);
            player = null;
            PlayerController.instance = null;
            FollowerManager.instance = null;

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
        }
    }
}
