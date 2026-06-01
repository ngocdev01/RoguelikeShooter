

using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace NgocDev.Core.Config
{
    public class EditorPathAttribute : Attribute
    {
        public string filePath;
        public EditorPathAttribute(string filePath)
        {
            this.filePath = filePath;
        }
    }


    public abstract class ScriptableSetting<T> : ScriptableObject where T : ScriptableSetting<T>
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetOrCreateInstance();
                }
                return _instance;
            }
        }

        public static T GetOrCreateInstance()
        {
            T instance = null;
#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(GetFilePath()))) { return null; }
                var path = GetFilePath();
            instance = AssetDatabase.LoadAssetAtPath<T>(path);
            if (instance == null)
            {
                instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, path);
                AssetDatabase.SaveAssets();
                return instance;
            }
#endif
            return instance;

        }

#if UNITY_EDITOR
        public static SerializedObject GetSerializedObject()
        {
            return new SerializedObject(instance);
        }

#endif

        protected static string GetFilePath()
        {
            Type typeFromHandle = typeof(T);
            object[] customAttributes = typeFromHandle.GetCustomAttributes(inherit: true);
            object[] array = customAttributes;
            foreach (object obj in array)
            {
                if (obj is EditorPathAttribute)
                {
                    EditorPathAttribute editorPath = obj as EditorPathAttribute;
                    return editorPath.filePath;
                }
            }

            return string.Empty;
        }
    }

}
