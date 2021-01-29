namespace GGJ2021
{
    using UnityEngine;

    public class PlayerGrappleManager
    {
        private LineRenderer grapple;

        private PlayerController playerController;
        private Vector2 playerPos, destination, direction;
        private float grappleLength;
        private float curGrappleTime, maxGrappleTime;

        public PlayerGrappleManager(PlayerController playerController, LineRenderer grapple)
        {
            this.playerController = playerController;
            this.grapple = grapple;
        }

        public void OnUpdate(float deltaTime)
        {
            playerPos = playerController.transform.position;
            if (curGrappleTime > 0)
            {
                curGrappleTime = Mathf.Max(0, curGrappleTime - deltaTime);
                if (curGrappleTime <= 0)
                {
                    ResetGrapple();
                }
                else
                {
                    DrawGrapple();
                }
            }
        }

        public bool Grapple()
        {
            direction = playerController.lookDirection;
            grappleLength = playerController.config.grappleRange;
            destination = playerPos + direction.normalized * grappleLength;
            curGrappleTime = playerController.config.grappleFailRetractTime;
            maxGrappleTime = playerController.config.grappleFailRetractTime;
            return false;
        }

        private void DrawGrapple()
        {
            grapple.SetPosition(0, playerPos);
            grapple.SetPosition(1, Vector2.Lerp(playerPos, destination, curGrappleTime/maxGrappleTime));
        }

        private void ResetGrapple()
        {
            grapple.SetPosition(0, Vector2.zero);
            grapple.SetPosition(1, Vector2.zero);
        }

        public bool CanGrapple()
        {
            return curGrappleTime <= 0f;
        }
    }
}
