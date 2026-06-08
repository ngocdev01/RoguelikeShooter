using UnityEngine;
using UnityEngine.AddressableAssets;

using System;
using System.Collections.Generic;
using System.Linq;

namespace NgocDev.Core.Config
{
    [ScriptableSetting("Config/GameConfig","Assets/Config/GameConfig")]
    public class GameConfig : ScriptableSetting<GameConfig>
    {
        public bool useBoostrapScene = true;
        public SceneReference bootstrapScene = null;
        public string assetsPath = "Assets/AddressableAssets";
        public string eventChannelFolder = null;

    }

 

}