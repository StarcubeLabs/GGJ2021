namespace GGJ2021
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class PrefabListStorage : SerializableDictionary.Storage<List<GameObject>>
    {
    }
}
