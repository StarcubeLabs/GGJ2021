namespace GGJ2021
{
    using UnityEngine;

    public class PlayerCamera : MonoBehaviour
    {

        public Vector3 offset;

        // Update is called once per frame
        void Update()
        {
            if (PlayerController.instance == null)
            {
                return;
            }
            transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, transform.position.z);
            transform.position += offset;
        }
    }
}
