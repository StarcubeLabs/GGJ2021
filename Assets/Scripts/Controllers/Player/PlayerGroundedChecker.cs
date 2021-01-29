namespace GGJ2021
{
    using UnityEngine;

    public class PlayerGroundedChecker : MonoBehaviour
    {
        public SpriteRenderer groundedIndicator;
        public Color grounded, sliding, inAir;

        public PlayerController playerController;

        // Update is called once per frame
        void Update()
        {
            if (playerController.playerCollision.IsGrounded())
            {
                groundedIndicator.color = grounded;
            }
            else if (playerController.playerCollision.IsSliding())
            {
                groundedIndicator.color = sliding;
            }
            else
            {
                groundedIndicator.color = inAir;
            }
        }
    }
}
