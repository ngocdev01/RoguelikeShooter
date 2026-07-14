using UnityEngine;
using UnityEngine.AddressableAssets;

using System;
using System.Collections.Generic;
using System.Linq;

namespace NgocDev.Core.Config
{
    [ScriptableSetting("Assets/Config/GameConfig", "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public SceneReference bootstrapScene = null;
        public string assetsPath = "Assets/AddressableAssets";
        public string eventChannelFolder = null;

    }

 

}