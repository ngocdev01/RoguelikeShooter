using UnityEditor;
using UnityEngine;

namespace NgocDev.Gameplay.MapGeneration.Editor
{
    using NgocDev.Editor;
    using NgocDev.Gameplay.MapGeneration;
    using NgocDev.Gameplay.MapGeneration.Editor;
    using NgocDev.Gameplay.MapGeneration.Editor.MapEditor;
    using System;
    using UnityEditor.EditorTools;
    using UnityEditor.ShortcutManagement;
    using UnityEngine.Rendering;

    [EditorTool("Map Paint Tool", targetContext = typeof(MapEditorToolContext))]
    public class MapPaintTool : EditorTool
    {
        private TilePrefab _selectedTilePrefab;

        private bool _isPainting = false;
        private bool _hasChange = false;



        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("Grid.PaintTool");

        [Shortcut("Map Editor/Select Paint Tool", typeof(MapEditorShortcutContext), KeyCode.P)]
        public static void ShortCut(ShortcutArguments args)
        {
            ToolManager.SetActiveTool<MapPaintTool>();
        }


        public override void OnToolGUI(EditorWindow window)
        {
            if (window is not SceneView view)
                return;
            HandleMouseInput(view);


        }

        private void HandleMouseInput(SceneView view)
        {
            var e = Event.current;
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            var target = MapEditorManager.activeTarget;
            Plane plane = new Plane(Vector3.up, target.grid.origin);

            if (HandleUtility.PlaceObject(e.mousePosition, out Vector3 hit, out Vector3 normal))
            {
                hit += normal * target.grid.cellSize / 2;
            }
            else if (HandlesExtensions.ScreenRaycastPlane(plane, out hit))
            {
                hit += plane.normal * target.grid.cellSize / 2;

            }
            hit = Grid.SnapToGridCenter(target.grid, hit);
            Handles.color = MapEditorSetting.previewColor;
            if (MapEditorManager.activeTile == null)
            {
                Debug.LogWarning("No active tile selected to paint.");
                _isPainting = false;
                return;
            }
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(MapEditorManager.activeTile.editorPath);
            MapEditorUtility.DrawPreview(obj, hit);
            view.Repaint();



            if (e.type == EventType.MouseDown && !e.alt && !e.control)
            {
                _isPainting = true;


            }

            if (e.type == EventType.MouseUp && _isPainting)
            {
                Undo.RecordObject(MapEditorManager.activeTarget, "Paint Tile");
                var tileData = new TileData
                {
                    prefab = MapEditorManager.activeTile,
                    rotation = 0,
                    scale = 0
                };
                MapEditorManager.activeTarget.SetTilePrefabAt(Grid.WorldToGrid(target.grid, hit), tileData);
                UnityEditor.EditorUtility.SetDirty(MapEditorManager.activeTarget);
                _isPainting = false;
            }
        }


    }

    [EditorTool("Map Erase Tool", targetContext = typeof(MapEditorToolContext))]

    public class MapEraseTool : EditorTool
    {
        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("Grid.EraserTool");

        [Shortcut("Map Editor/Select Erase Tool", typeof(MapEditorShortcutContext), KeyCode.E)]
        public static void ShortCut(ShortcutArguments args)
        {
            ToolManager.SetActiveTool<MapEraseTool>();
        }
    }


    [EditorTool("Map Fill Tool", targetContext = typeof(MapEditorToolContext))]
    public class MapFillTool : EditorTool
    {
        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("Grid.FillTool");
        [Shortcut("Map Editor/Select Fill Tool", typeof(MapEditorShortcutContext), KeyCode.F)]
        public static void ShortCut(ShortcutArguments args)
        {
            ToolManager.SetActiveTool<MapFillTool>();
        }
    }

    [EditorTool("Map Box Fill Tool", targetContext = typeof(MapEditorToolContext))]
    public class MapBoxFillTool : EditorTool
    {
        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("Grid.BoxTool");

        private bool _isPainting;
        private Vector3Int _startPos;
        private Vector3Int _currentPos;

        [Shortcut("Map Editor/Select Box Fill Tool", typeof(MapEditorShortcutContext), KeyCode.B)]
        public static void ShortCut(ShortcutArguments args)
        {
            ToolManager.SetActiveTool<MapBoxFillTool>();
        }

        public override void OnToolGUI(EditorWindow window)
        {
            HandleMouseInput(window as SceneView);
            DrawPreview(window as SceneView);
        }



        private void HandleMouseInput(SceneView view)
        {
            var target = MapEditorManager.activeTarget;
            var currentPos = Vector3Int.zero;
            var e = Event.current;

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt && !e.control)
            {
                _isPainting = true;
                _startPos = MapEditorUtility.GetMouseGridPosition(target.grid);
                _currentPos = _startPos;
                e.Use();
            }

            if (e.type == EventType.MouseDrag && _isPainting)
            {
                _currentPos = MapEditorUtility.GetMouseGridPosition(target.grid);
                e.Use();
            }

            if (e.type == EventType.MouseUp && _isPainting)
            {
                _isPainting = false;
                PlaceTile();
                e.Use();

            }
            if (_isPainting)
            {
                currentPos = MapEditorUtility.GetMouseGridPosition(target.grid);
            }
        }

        private void DrawPreview(SceneView sceneView)
        {
            if (!_isPainting)
                return;
            if (Event.current.type == EventType.Repaint)
            {

                Mesh mesh = MapEditorManager.activeTilePrefab.GetComponent<MeshFilter>().sharedMesh;
                Material material = MapEditorSetting.previewMaterial;
                Matrix4x4 matrix = Matrix4x4.Translate(Grid.GridCenterToWorld(MapEditorManager.activeTarget.grid, _currentPos));

                int totalTiles = GetBox(_startPos, _currentPos, out Vector3Int[] positions);
                if (totalTiles == 0)
                    return;
                Matrix4x4[] tilesMatrix = new Matrix4x4[totalTiles];

                for (int i = 0; i < totalTiles; i++)
                {
                    tilesMatrix[i] = Matrix4x4.Translate(Grid.GridCenterToWorld(MapEditorManager.activeTarget.grid, positions[i]));

                }



                RenderParams renderParams = new RenderParams(material)
                {
                    camera = sceneView.camera,
                    layer = 0,

                };

                Graphics.RenderMeshInstanced(renderParams, mesh, 0, tilesMatrix);
            }
        }

        private int GetBox(Vector3Int pos1, Vector3Int pos2, out Vector3Int[] tiles)
        {
            Vector3Int min = Vector3Int.Min(pos1, pos2);
            Vector3Int max = Vector3Int.Max(pos1, pos2);
            Vector3Int box = max - min + Vector3Int.one;
            int size = box.x * box.y * box.z;
            tiles = new Vector3Int[size];
            int index = 0;
            for (int x = min.x; x <= max.x; x++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int z = min.z; z <= max.z; z++)
                    {
                        tiles[index++] = new Vector3Int(x, y, z);
                    }
                }
            }
            return size;
        }

        private void PlaceTile()
        {
            var tile = MapEditorManager.activeTile;
            if (tile == null) return;
            var size = GetBox(_startPos, _currentPos, out Vector3Int[] positions);
            Undo.RecordObject(MapEditorManager.activeTarget, "Box Fill Tiles");
            for (int i = 0; i < size; i++)
            {
                var tileData = new TileData
                {
                    prefab = tile,
                    rotation = 0,
                    scale = 0
                };
                MapEditorManager.activeTarget.SetTilePrefabAt(positions[i], tileData);
            }

        }
    }
}