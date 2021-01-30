namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerGrappleManager
    {
        private LineRenderer grapple;

        private PlayerController playerController;
        private Vector2 playerPos, grappleDestination, direction, playerDestination;
        private float grappleLength;
        private float curGrappleFailTime, maxGrappleFailTime;

        private List<Vector2> offsets = new List<Vector2>();
        private RaycastHit2D hit;

        public PlayerGrappleManager(PlayerController playerController, LineRenderer grapple)
        {
            this.playerController = playerController;
            this.grapple = grapple;
            offsets.Add(new Vector2(0f, 0f));
            offsets.Add(new Vector2( playerController.config.grappleScatterOffset, 0f));
            offsets.Add(new Vector2(-playerController.config.grappleScatterOffset, 0f));
            offsets.Add(new Vector2(0f,  playerController.config.grappleScatterOffset));
            offsets.Add(new Vector2(0f, -playerController.config.grappleScatterOffset));
        }

        public void OnUpdate(float deltaTime)
        {
            playerPos = playerController.transform.position;
            if (curGrappleFailTime > 0)
            {
                curGrappleFailTime = Mathf.Max(0, curGrappleFailTime - deltaTime);
                if (curGrappleFailTime <= 0)
                {
                    ResetGrapple();
                }
                else
                {
                    DrawGrappleFail();
                }
            }
        }

        public bool TryGrapple()
        {
            playerPos = playerController.transform.position;
            direction = playerController.lookDirection;
            grappleLength = playerController.config.grappleRange;
            grappleDestination = playerPos + direction.normalized * grappleLength;
            foreach (Vector2 offset in offsets)
            {
                direction = (direction + offset).normalized;
                hit = Physics2D.Raycast(playerPos, direction, grappleLength, playerController.config.grappleLayerMask);
                if (hit)
                {
                    grappleDestination = hit.collider.gameObject.transform.position;
                    playerDestination = hit.collider.gameObject.GetComponent<GrapplePoint>().destination.position;
                    return true;
                }
            }
            curGrappleFailTime = playerController.config.grappleFailRetractTime;
            maxGrappleFailTime = playerController.config.grappleFailRetractTime;
            return false;
        }

        private void DrawGrappleFail()
        {
            grapple.SetPosition(0, playerPos);
            grapple.SetPosition(1, Vector2.Lerp(playerPos, grappleDestination, curGrappleFailTime/maxGrappleFailTime));
        }

        public void DrawGrappleSuccess()
        {
            grapple.SetPosition(0, playerPos);
            grapple.SetPosition(1, grappleDestination);
        }

        public void ResetGrapple()
        {
            grapple.SetPosition(0, Vector2.zero);
            grapple.SetPosition(1, Vector2.zero);
        }

        public bool CanGrapple()
        {
            return curGrappleFailTime <= 0f;
        }

        public Vector2 GetPlayerDesition()
        {
            return playerDestination;
        }
    }
}
