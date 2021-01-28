using UnityEditor;
using UnityEngine;

namespace GGJ2021
{
    /// <summary>
    /// Editor tool to toggle the 3D tilemap between sprites and objects.
    /// </summary>
    public class Toggle3DTilemap : MonoBehaviour
    {

        [MenuItem("Tilemap/Toggle 3D Tilemap")]
        static void ToggleTilemap()
        {
            Tilemap3D tilemap3D = FindObjectOfType<Tilemap3D>();
            if (tilemap3D == null)
            {
                Debug.LogError("No Tilemap3D found.");
            }
            else
            {
                tilemap3D.ToggleObjects();
            }
        }
    }
}
