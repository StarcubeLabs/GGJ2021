namespace GGJ2021
{
    using UnityEngine;
    using Rewired;

    public class RewiredPlayerInputManager : MonoBehaviour
    {
        public static RewiredPlayerInputManager instance;

        // The Rewired player id. Currently, this will always be 0.
        private int playerId = 0;

        // The Rewired Player
        private Player player;

        private bool debug = false;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
            player = ReInput.players.GetPlayer(playerId);
        }

        private void Update()
        {
            if (debug)
            {
                if (GetHorizontalMovement() != 0f || GetVerticalMovement() != 0f)
                {
                    Debug.Log("GetHorizontalMovement: " + GetHorizontalMovement());
                    Debug.Log("GetVerticalMovement: " + GetVerticalMovement());
                }

                if (ADown())
                {
                    Debug.Log("A down!");
                }
            }
        }

        public float GetHorizontalMovement()
        {
            return player.GetAxis("MoveHorizontal");
        }

        public float GetVerticalMovement()
        {
            return player.GetAxis("MoveVertical");
        }

        public float GetHorizontalMovement2()
        {
            return player.GetAxis("MoveHorizontal2");
        }

        public float GetVerticalMovement2()
        {
            return player.GetAxis("MoveVertical2");
        }

        public bool ADown()
        {
            return player.GetButtonDown("ADown");
        }

        public bool BDown()
        {
            return player.GetButtonDown("BDown");
        }

        public bool XDown()
        {
            return player.GetButtonDown("XDown");
        }

        public bool YDown()
        {
            return player.GetButtonDown("YDown");
        }

        public bool L1Down()
        {
            return player.GetButtonDown("L1Down");
        }

        public bool L2Down()
        {
            return player.GetButtonDown("L2Down");
        }

        public bool R1Down()
        {
            return player.GetButtonDown("R1Down");
        }

        public bool R2Down()
        {
            return player.GetButtonDown("R2Down");
        }

        public bool StartDown()
        {
            return player.GetButtonDown("StartDown");
        }

        public bool IsFiring()
        {
            return R1Down();
        }

        public bool IsDashing()
        {
            return L2Down() || BDown();
        }

        public bool IsJumping()
        {
            return L1Down() || ADown();
        }
    }
}