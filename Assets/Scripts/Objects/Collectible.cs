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
        private string id;
        public string Id
        {
            get { return id; }
        }

        private void Start()
        {
            if (PlayerStats.instance.IsCollectibleCollected(this))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerStats.instance.CollectCollectible(this);
            OnCollect();
            gameObject.SetActive(false);
        }

        protected abstract void OnCollect();

        public void InitFromTilemap(Tilemap3D tilemap)
        {
            id = tilemap.GetComponent<CollectibleIndexer>().GenerateId(this);
        }
    }
}
