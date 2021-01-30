namespace GGJ2021
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Editor tool to toggle the 3D tilemap between sprites and objects.
    /// </summary>
    public class Toggle3DTilemap : MonoBehaviour
    {

        [MenuItem("Tilemap/Toggle 3D Tilemap")]
        static void ToggleTilemap()
        {
            Tilemap3D[] tilemaps = FindObjectsOfType<Tilemap3D>();
            if (tilemaps.Length == 0)
            {
                Debug.LogError("No Tilemap3D found.");
            }
            else
            {
                foreach (Tilemap3D tilemap in tilemaps)
                {
                    tilemap.ToggleObjects();
                }
            }
        }
    }
}
