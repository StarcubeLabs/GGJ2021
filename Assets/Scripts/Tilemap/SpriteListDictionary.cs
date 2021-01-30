namespace GGJ2021
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SpriteListDictionary : SerializableDictionary<Sprite, List<GameObject>, PrefabListStorage>
    {
    }
}
