namespace GGJ2021.Management
{

    /// <summary>
    /// Handles transitioning to the end of the game.
    /// </summary>
    class EndGame : SceneTransition
    {

        public override void LoadLevel()
        {
            PlayerStats.instance.Reset();
            base.LoadLevel();
        }
    }
}