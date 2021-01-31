namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    /// <summary>
    /// Maps a tilemap to model prefabs.
    /// </summary>
    public class Tilemap3D : MonoBehaviour
    {

        private const int UP_DIR_MASK = 1;
        private const int RIGHT_DIR_MASK = 1 << 1;
        private const int DOWN_DIR_MASK = 1 << 2;
        private const int LEFT_DIR_MASK = 1 << 3;

        /// <summary>
        /// One-to-one sprite-to-prefab mappings.
        /// </summary>
        [SerializeField]
        private SpritePrefabDictionary spritesToPrefabs;
        /// <summary>
        /// Sprite-to-prefabs mappings where blocks are shaded differently depending on neighboring blocks.
        /// Block indices correspond to bitmasks where bits correspond to whether there is a neighboring block.
        /// E.g., index 9 (b1001) for the bottom-right corner block, with neighboring blocks up and left.
        /// </summary>
        [SerializeField]
        private SpriteListDictionary shadedPrefabs;
        /// <summary>
        /// Bit masks for sprites with special collision data (not all directions on/off).
        /// </summary>
        [SerializeField]
        private SpriteIntDictionary prefabShadingMasks;
        private Tilemap tileMap;

        private bool ObjectsEnabled
        {
            get { return transform.childCount > 0; }
        }

        private void Update()
        {
            if (Application.isPlaying && !ObjectsEnabled)
            {
                CreateObjects();
            }
        }

        private void CreateObjects()
        {
            tileMap = GetComponent<Tilemap>();
            BoundsInt cellBounds = tileMap.cellBounds;
            for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
            {
                for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    Sprite sprite = tileMap.GetSprite(cellPos);
                    if (sprite != null)
                    {
                        GameObject prefab = null;
                        int addedRotation = 0;
                        Quaternion spriteRotationQuat = tileMap.GetTransformMatrix(cellPos).rotation;
                        float spriteRotation = spriteRotationQuat.eulerAngles.z;
                        int flipped = spriteRotationQuat.eulerAngles.y > 1 ? -1 : 1;
                        if (spritesToPrefabs.ContainsKey(sprite))
                        {
                            prefab = spritesToPrefabs[sprite];
                        }
                        else if (shadedPrefabs.ContainsKey(sprite))
                        {
                            List<GameObject> shadedPrefabList = shadedPrefabs[sprite];
                            int neighborFlags = 0;
                            // Form a bitmask representing whether there is a neighboring block in each direction.
                            // Bit 0: Up
                            // Bit 1: Right
                            // Bit 2: Down
                            // Bit 3: Left
                            int originMask = 0;
                            int bitRotate = Mathf.RoundToInt(spriteRotation / 90);
                            if (prefabShadingMasks.ContainsKey(sprite))
                            {
                                originMask = prefabShadingMasks[sprite];
                                originMask = FlipHorizontalDirections(originMask, flipped);
                                originMask = BitRotateRight(originMask, bitRotate * flipped);
                            }
                            neighborFlags += HasBlock(originMask, x, y + 1, UP_DIR_MASK);
                            neighborFlags += HasBlock(originMask, x + 1, y, RIGHT_DIR_MASK) << 1;
                            neighborFlags += HasBlock(originMask, x, y - 1, DOWN_DIR_MASK) << 2;
                            neighborFlags += HasBlock(originMask, x - 1, y, LEFT_DIR_MASK) << 3;

                            // Rotate the neighboring flags according to the sprite's rotation.
                            neighborFlags = FlipHorizontalDirections(neighborFlags, flipped);
                            neighborFlags = BitRotateLeft(neighborFlags, bitRotate);
                            for (int i = 0; i < 4; i++)
                            {
                                // Look through all four rotations of the shading in the list.
                                int rotatedFlags = BitRotateLeft(neighborFlags, i);
                                if (rotatedFlags < shadedPrefabList.Count && shadedPrefabList[rotatedFlags] != null)
                                {
                                    prefab = shadedPrefabList[rotatedFlags];
                                    addedRotation = i * 90;
                                    break;
                                }
                            }
                            if (prefab == null)
                            {
                                // If no shaded prefab was found, default to the unshaded one.
                                prefab = shadedPrefabList[0];
                            }
                        }
                        if (prefab == null)
                        {
                            continue;
                        }
                        GameObject newObject = Instantiate(prefab, tileMap.GetCellCenterLocal(cellPos),
                        prefab.transform.rotation * spriteRotationQuat * Quaternion.Euler(0, 0, addedRotation), transform);

                        TilemapInit init = newObject.GetComponent(typeof(TilemapInit)) as TilemapInit;
                        if (init != null)
                        {
                            init.InitFromTilemap(this);
                        }
                    }
                }
            }
            if (transform.childCount > 0)
            {
                GetComponent<TilemapRenderer>().enabled = false;
            }
        }

        public void ToggleObjects()
        {
            if (ObjectsEnabled)
            {
                DestroyObjects();
            }
            else
            {
                CreateObjects();
            }
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

        private int HasBlock(int originMask, int x, int y, int dirMask)
        {
            BoundsInt cellBounds = tileMap.cellBounds;
            if (x < cellBounds.xMin || x >= cellBounds.xMax || y < cellBounds.yMin || y >= cellBounds.yMax)
            {
                return 0;
            }

            if ((originMask & dirMask) == 0)
            {
                return 0;
            }

            Vector3Int neighborSpritePos = new Vector3Int(x, y, 0);
            Sprite neighborSprite = tileMap.GetSprite(neighborSpritePos);

            if (neighborSprite != null && prefabShadingMasks.ContainsKey(neighborSprite))
            {
                // Rotate the shading mask according to the neighbor sprite's rotation.
                Vector3 spriteRotation = tileMap.GetTransformMatrix(neighborSpritePos).rotation.eulerAngles;
                float spriteRotationZ = tileMap.GetTransformMatrix(neighborSpritePos).rotation.eulerAngles.z;
                int bitRotate = Mathf.RoundToInt(spriteRotationZ / 90);
                int shadingMask = prefabShadingMasks[neighborSprite];
                int flipped = spriteRotation.y > 1 ? -1 : 1;
                shadingMask = FlipHorizontalDirections(shadingMask, flipped);
                shadingMask = BitRotateRight(shadingMask, bitRotate * flipped);

                // If the shading mask bit in the given direction is on, there is a block in that direction.
                return (BitRotateLeft(dirMask, 2) & shadingMask) == 0 ? 0 : 1;
            }
            return 0;
        }

        /// <summary>
        /// Flips the left and right directions on a bitmask (bits 1 and 3).
        /// </summary>
        private int FlipHorizontalDirections(int value, int flipped)
        {
            if (flipped == -1)
            {
                return value & 5 | BitRotateLeft(value & 10, 2);
            }
            return value;
        }
        /// <summary>
        /// Rotates the direction mask left (counter-clockwise, positive rotation).
        /// </summary>
        private int BitRotateLeft(int value, int count)
        {
            if (count < 0)
            {
                return BitRotateRight(value, -count);
            }
            return ((value << count) | (value >> (4 - count))) & 15;
        }

        /// <summary>
        /// Rotates the direction mask right (clockwise, negative rotation).
        /// </summary>
        private int BitRotateRight(int value, int count)
        {
            if (count < 0)
            {
                return BitRotateLeft(value, -count);
            }
            return ((value >> count) | (value << (4 - count))) & 15;
        }
    }
}
