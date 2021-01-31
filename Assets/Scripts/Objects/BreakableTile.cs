using UnityEngine;

namespace GGJ2021
{

    /// <summary>
    /// Block that can be destroy by grenades.
    /// </summary>
    public class BreakableTile : MonoBehaviour
    {

        public void Break()
        {
            //TODO Animate destruction.
            Destroy(gameObject);
        }
    }
}