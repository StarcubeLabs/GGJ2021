namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// Base class for any collectible item.
    /// </summary>
    public abstract class Collectible : MonoBehaviour, TilemapInit
    {
        [Tooltip("Unique ID of the health extension to track collection.")]
        [SerializeField]
        protected string id;
        public string Id
        {
            get { return id; }
        }

        private bool initialized = false;

        private void Start()
        {
            TryInit();
        }

        private void Update()
        {
            TryInit();
        }

        private void TryInit()
        {
            if (!initialized && PlayerStats.instance != null)
            {
                if (PlayerStats.instance.IsCollectibleCollected(this))
                {
                    gameObject.SetActive(false);
                }
                initialized = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerStats.instance.CollectCollectible(this);
            OnCollect();
            gameObject.SetActive(false);
        }

        protected abstract void OnCollect();

        public virtual void InitFromTilemap(Tilemap3D tilemap)
        {
            if (id == null || id == "")
            {
                id = tilemap.GetComponent<CollectibleIndexer>().GenerateId(this);
            }
        }
    }
}
