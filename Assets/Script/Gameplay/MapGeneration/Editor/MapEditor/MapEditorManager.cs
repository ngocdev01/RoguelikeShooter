using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace NgocDev.Gameplay.MapGeneration.Editor
{
    public class MapEditorManager : ScriptableSingleton<MapEditorManager>
    {
        [SerializeField]
        private Map _activeTarget;
        [SerializeField]
        private TilePrefab _activeTile;
        private GameObject _activeTilePrefab;

        private KeyValuePair<TilePrefab, GameObject> _cachedActiveTile;

        public static Map activeTarget { get => instance._activeTarget; internal set => instance._activeTarget = value; }
        public static TilePrefab activeTile {
            get => instance._activeTile;
            internal set => instance._activeTile = value;
        }

        public static GameObject activeTilePrefab
        {
            get
            {
                if (instance._activeTile == null)
                    return null;
                if(instance._cachedActiveTile.Key != instance._activeTile)
                {
                    instance._cachedActiveTile = new KeyValuePair<TilePrefab, GameObject>(
                        instance._activeTile,
                        AssetDatabase.LoadAssetAtPath<GameObject>(instance._activeTile.editorPath)
                    );
                }
                return instance._cachedActiveTile.Value;
            }
        }

        public static void OpenMapEditor(string mapPath)
        {
            instance._activeTarget = AssetDatabase.LoadAssetAtPath<Map>(mapPath);
            var stage = MapEditorPreviewSceneStage.Create(mapPath);
            StageUtility.GoToStage(stage, true);

        }
        public static void OpenMapEditor(Map map)
        {
            OpenMapEditor(AssetDatabase.GetAssetPath(map));
        }

        public static void CloseMapEditor()
        {
            StageUtility.GoToMainStage();
        }

        
    }

}
