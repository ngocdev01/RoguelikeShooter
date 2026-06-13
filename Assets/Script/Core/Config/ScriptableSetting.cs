

using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace NgocDev.Core.Config
{
    public class ScriptableSettingAttribute : Attribute
    {
        public string editorFilePath;
        public string runtimeKey;
        public ScriptableSettingAttribute(string editorFilePath, string runtimeKey)
        {
            this.editorFilePath = editorFilePath;
            this.runtimeKey = runtimeKey;
        }
    }




    public abstract class ScriptableSetting<T> : ScriptableObject where T : ScriptableSetting<T>
    {
        private static T _instance;
        protected string _runtimeKey;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetInstance();
                }
                return _instance;
            }
        }

        public static T GetInstance()
        {
            var type = typeof(T);
            var attributes = type.GetCustomAttribute<ScriptableSettingAttribute>(false);
#if UNITY_EDITOR
            var editorFilePath = attributes?.editorFilePath;
            var asset = AssetDatabase.LoadAssetAtPath<T>(editorFilePath);
            if (asset == null)
            {
                Debug.LogError($"Failed to load ScriptableSetting at path: {editorFilePath}");
            }
            return asset;
#else
            var runtimeKey = attributes?.runtimeKey;
            //TODO: Async load
            var asset = Addressables.LoadAssetAsync<T>(runtimeKey).WaitForCompletion();
            if(asset == null)
            {
                Debug.LogError($"Failed to load ScriptableSetting with key: {runtimeKey}");
            }
            return asset;
#endif
        }

        public async Awaitable InitializeAsync()
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttribute<ScriptableSettingAttribute>(false);
            if (attribute == null)
            {
                Debug.LogError($"ScriptableSetting of type {type.Name} is missing ScriptableSettingAttribute.");
            }
            var runtimeKey = attribute.runtimeKey;
            _instance = await Addressables.LoadAssetAsync<T>(runtimeKey).Task;
        }
    }
}
