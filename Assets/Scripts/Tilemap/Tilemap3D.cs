using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Starjam
{
    /// <summary>
    /// Maps a tilemap to model prefabs.
    /// </summary>
    public class Tilemap3D : MonoBehaviour
    {

        [SerializeField]
        private SpritePrefabDictionary spritesToPrefabs;
        private List<GameObject> instantiatedPrefabs = new List<GameObject>();

        void Start()
        {
            createPrefabs();
            GetComponent<TilemapRenderer>().enabled = false;
        }

        private void createPrefabs()
        {
            Tilemap tileMap = GetComponent<Tilemap>();
            BoundsInt cellBounds = tileMap.cellBounds;
            for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
            {
                for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    Sprite sprite = tileMap.GetSprite(cellPos);
                    if (sprite != null)
                    {
                        GameObject prefab = spritesToPrefabs[sprite];
                        if (prefab == null)
                        {
                            Debug.LogError("Missing prefab for tile " + sprite);
                        }
                        GameObject prefabInstance = Instantiate(prefab, tileMap.GetCellCenterWorld(cellPos),
                            prefab.transform.rotation * tileMap.GetTransformMatrix(cellPos).rotation);
                        instantiatedPrefabs.Add(prefabInstance);
                    }
                }
            }
        }
    }

}
