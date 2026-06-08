
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace NgocDev.Gameplay.MapGeneration.Editor
{
    public class MapEditorPreviewSceneStage : UnityEditor.SceneManagement.PreviewSceneStage
    {
        [SerializeField]
        private SceneAsset _defaultEditScene = default;
        private string _mapPath;
        public override string assetPath => _mapPath;
        public GameObject mapRoot;

        private Dictionary<Vector3Int, GameObject> _renderedTiles = new Dictionary<Vector3Int, GameObject>();

        public static MapEditorPreviewSceneStage Create(string path)
        {
            var instance = CreateInstance<MapEditorPreviewSceneStage>();
            instance._mapPath = path;
            return instance;
        }

        protected override GUIContent CreateHeaderContent()
        {
            return new GUIContent("Map Editor Preview");
        }


        protected override bool OnOpenStage()
        {
            var path = AssetDatabase.GetAssetPath(_defaultEditScene);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Default edit scene is not set.");
                return false;
            }
            MapEditorSetting.Save();
            scene = UnityEditor.SceneManagement.EditorSceneManager.OpenPreviewScene(path);
            var rootObjects = scene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                SetObject(obj);
            }
            ToolManager.SetActiveContext<MapEditorToolContext>();
            mapRoot = new GameObject("MapRoot");
            mapRoot.hideFlags = HideFlags.NotEditable;
            SceneManager.MoveGameObjectToScene(mapRoot, scene);
           

            _ = BatchInstantiate(100);

            return true;
        }



        private void SetObject(GameObject go)
        {
            go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideAndDontSave | HideFlags.NotEditable;
            foreach (Transform obj in go.transform)
            {
                SetObject(obj.gameObject);
            }
        }

        protected override void OnCloseStage()
        {
            if (scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager.ClosePreviewScene(scene);
            }

            ToolManager.SetActiveContext<GameObjectToolContext>();
            Tools.current = Tool.Move;
            SceneView.duringSceneGui -= Render;
        }

        private void Render(SceneView view)
        {

            var map = MapEditorManager.activeTarget;
            var hasChange = false;
            foreach (var tile in map.tiles.dictionary)
            {
                if (!_renderedTiles.ContainsKey(tile.Key))
                {
                    var tileData = tile.Value;
                    var position = NgocDev.Gameplay.MapGeneration.Grid.GridCenterToWorld(map.grid, tile.Key);
                    var asset = AssetDatabase.LoadAssetAtPath<GameObject>(tileData.prefab.editorPath);
                    var go = Instantiate(asset, position, Quaternion.identity, mapRoot.transform);
                    go.name = $"Tile_{tile.Key.x}_{tile.Key.y}_{tile.Key.z}";
                    _renderedTiles[tile.Key] = go;
                    hasChange = true;
                }
            }
            if (hasChange)
            {
                view.Repaint();
            }
        }
        public async Awaitable BatchInstantiate(int batchSize)
        {
            var tiles = MapEditorManager.activeTarget.tiles;
            for (int i = 0; i < tiles.dictionary.Count; i += batchSize)
            {
                int currentBatchSize = Mathf.Min(batchSize, tiles.dictionary.Count - i);
                for (int j = 0; j < currentBatchSize; j++)
                {
                    var kvp = new List<KeyValuePair<Vector3Int, TileData>>(tiles.dictionary)[i + j];
                    var worldPos = Grid.GridCornerToWorld(MapEditorManager.activeTarget.grid, kvp.Key);
                    var prefab = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(kvp.Value.prefab.editorPath),
                        worldPos, Quaternion.identity);
                    _renderedTiles[kvp.Key] = prefab;
                }
                await Awaitable.NextFrameAsync();
            }

        }

    }
}
