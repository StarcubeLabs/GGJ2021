namespace GGJ2021.Management
{
    using UnityEngine;

    /// <summary>
    /// A point where the player can respawn after dying.
    /// </summary>
    class RespawnPoint : SceneTransition
    {

        public override void Transition()
        {
            PlayerStats.instance.spawnDoor = doorIndex;
        }
    }
}