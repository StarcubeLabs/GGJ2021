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
            { "placeholder", "event:/music/placeholder_music/placeholder_music" },
            #endregion
        };

        //Event name-to-fmod-path dictionary for SFX. Used to generate our fmod event pools.
        public Dictionary<string, string> fmodSFXEventDictionary = new Dictionary<string, string>()
        {
            //{ "", "" },

            #region Global
            { "PLACEHOLDER", "event:/PLACEHOLDER" },
            { "PLACEHOLDER_tonal", "event:/PLACEHOLDER_tonal" },
            #endregion
        };

        //Param name-to-fmod-name dictionary
        public Dictionary<string, string> fmodParamDictionary = new Dictionary<string, string>()
        {
            //{ "", "" },

            #region Global
            {"global_pitch_param", "global_pitch" },
            {"global_octave_param", "global_octave" },
            #endregion
        };
    }
}
