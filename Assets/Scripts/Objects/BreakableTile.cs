using UnityEngine;

namespace GGJ2021
{

    /// <summary>
    /// Block that can be destroy by grenades.
    /// </summary>
    public class BreakableTile : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            {
                Break();
            }
        }

        private void Break()
        {
            //TODO Animate destruction.
            Destroy(gameObject);
        }
    }
}