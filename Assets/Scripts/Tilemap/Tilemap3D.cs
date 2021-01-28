using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GGJ2021
{
    /// <summary>
    /// Maps a tilemap to model prefabs.
    /// </summary>
    public class Tilemap3D : MonoBehaviour
    {

        [SerializeField]
        private SpritePrefabDictionary spritesToPrefabs;

        private bool objectsEnabled
        {
            get { return transform.childCount > 0; }
        }

        private void Start()
        {
            if (Application.isPlaying && !objectsEnabled)
            {
                CreateObjects();
            }
        }

        private void CreateObjects()
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
                        Instantiate(prefab, tileMap.GetCellCenterLocal(cellPos),
                            prefab.transform.rotation * tileMap.GetTransformMatrix(cellPos).rotation, transform);
                    }
                }
            }
            GetComponent<TilemapRenderer>().enabled = false;
        }

        private void DestroyObjects()
        {
            List<GameObject> tileObjects = new List<GameObject>();
            foreach (Transform tileTransform in transform)
            {
                tileObjects.Add(tileTransform.gameObject);
            }
            foreach (GameObject tileObject in tileObjects)
            {
                DestroyImmediate(tileObject);
            }
            GetComponent<TilemapRenderer>().enabled = true;
        }

        public void ToggleObjects()
        {
            if (objectsEnabled)
            {
                DestroyObjects();
            }
            else
            {
                CreateObjects();
            }
        }
    }
}
