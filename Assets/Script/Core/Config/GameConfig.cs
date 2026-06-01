using UnityEngine;
using UnityEngine.AddressableAssets;

using System;
using System.Collections.Generic;
using System.Linq;

namespace NgocDev.Core.Config
{
    [EditorPath("Assets/Config/GameConfig.asset")]
    public class GameConfig : ScriptableSetting<GameConfig>
    {
        public bool useBoostrapScene = true;
        public SceneReference bootstrapScene = null;
        public string assetsPath = "Assets/AddressableAssets";
        public string eventChannelFolder = null;

    }

    public abstract class GameConfig<T> : ScriptableSetting<T> where T : GameConfig<T>
    {

    }

    public class GameConfigTreeNode
    {
        public Type configType;
        public ScriptableObject instance;
        public GameConfigTreeNode parent;
        public List<GameConfigTreeNode> children = new List<GameConfigTreeNode>();

        public GameConfigTreeNode(Type configType, ScriptableObject instance = null)
        {
            this.configType = configType;
            this.instance = instance;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class GameConfigAttribute : Attribute
    {
        public Type parent = null;
        public GameConfigAttribute(Type parent = null)
        {
            this.parent = parent;
        }
    }
}