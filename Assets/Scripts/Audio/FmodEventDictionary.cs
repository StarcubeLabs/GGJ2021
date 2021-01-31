namespace Audio
{
    using System.Collections.Generic;

    /// <summary>
    /// Class that allows us to access fmod event and param names by our own defined keys. This way if fmod event names or paths change, we need only update the FmodNameDictionary script.
    /// </summary>
    public class FmodEventDictionary
    {
        //Event name-to-fmod-path dictionary for Music.
        public Dictionary<string, string> fmodMusicEventDictionary = new Dictionary<string, string>()
        {
            //{ "", "" },

            #region Placeholder
            { "title_screen", "event:/music/title_screen" },
            { "level1", "event:/music/level1" },
            { "final_cutscene", "event:/music/final_cutscene" },
            #endregion
        };

        //Event name-to-fmod-path dictionary for SFX. Used to generate our fmod event pools.
        public Dictionary<string, string> fmodSFXEventDictionary = new Dictionary<string, string>()
        {
            //{ "", "" },

            #region Global
            { "hamster_dash", "event:/sfx/abilities/hamster_dash" },
            { "hamster_dash_tube_enter", "event:/sfx/abilities/hamster_dash_tube_enter" },
            { "hamster_dash_tube_exit", "event:/sfx/abilities/hamster_dash_tube_exit" },
            { "hamster_dash_tube_rolling", "event:/sfx/abilities/hamster_dash_tube_rolling" },
            { "hamster_explosion_fire", "event:/sfx/abilities/hamster_explosion_fire" },
            { "hamster_explosion_wet", "event:/sfx/abilities/hamster_explosion_wet" },
            { "hamster_grapple_hit", "event:/sfx/abilities/hamster_grapple_hit" },
            { "hamster_grapple_shoot", "event:/sfx/abilities/hamster_grapple_shoot" },
            { "hamster_grapple_zip", "event:/sfx/abilities/hamster_grapple_zip" },
            { "hamster_shoot", "event:/sfx/abilities/hamster_shoot" },
            { "player_damage", "event:/sfx/abilities/player_damage" },
            { "player_damage_acid", "event:/sfx/abilities/player_damage_acid" },
            { "player_death", "event:/sfx/abilities/player_death" },
            { "player_footsteps", "event:/sfx/abilities/player_footsteps" },
            { "player_jump", "event:/sfx/abilities/player_jump" },

            { "enemy_death", "event:/sfx/enemies/enemy_death" },
            { "enemy_shoot", "event:/sfx/enemies/enemy_shoot" },

            { "collect_hamster", "event:/sfx/misc/collect_hamster" },
            { "hamster_respawn", "event:/sfx/misc/hamster_respawn" },
            { "heal", "event:/sfx/misc/heal" },
            { "heal_power_up", "event:/sfx/misc/heal_power_up" },

            { "title_menu_button", "event:/ui/title_menu_button" },
            #endregion
        };

        //Param name-to-fmod-name dictionary
        public Dictionary<string, string> fmodParamDictionary = new Dictionary<string, string>()
        {
            //{ "", "" },

            #region Global
            {"battle", "battle" },
            #endregion
        };
    }
}
