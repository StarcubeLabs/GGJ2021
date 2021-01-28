namespace GGJ2021
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerCamera : MonoBehaviour
    {
        private PlayerController playerController;

        // Start is called before the first frame update
        void Start()
        {
            playerController = PlayerController.instance;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, transform.position.z);
        }
    }
}
