using System;
using UnityEditor;
using UnityEngine;

namespace NgocDev.Gameplay.MapGeneration.Editor
{

    using NgocDev.Gameplay.MapGeneration;
    using NgocDev.Gameplay.MapGeneration.Editor.MapEditor;
    using UnityEditor.EditorTools;
    using UnityEditor.SceneManagement;
    using UnityEditor.Search;
    using UnityEditor.ShortcutManagement;

    public class MapEditorShortcutContext : IShortcutContext
    {
        public bool active => ToolManager.activeContextType == typeof(MapEditorToolContext);

    }


    [EditorToolContext("Map Editor Tool")]
    public class MapEditorToolContext : EditorToolContext
    {
        public MapTilePaletteOverlay TilePallette { get; protected set; }


        private MapEditorShortcutContext _shortcutContext;
        public Color PreviewHandleColor = new Color(0.5f, 0.5f, 1f);

     
        

        public MapEditorToolContext()
        {           
            _shortcutContext = new MapEditorShortcutContext();
        }

        private void OnEnable()
        {
            TilePallette = new MapTilePaletteOverlay(MapEditorManager.activeTarget.tileMap);
            ShortcutManager.RegisterContext(_shortcutContext);
        }
        private void OnDisable()
        { 
            ShortcutManager.UnregisterContext(_shortcutContext);
        }



        public override void OnToolGUI(EditorWindow window)
        {
            if (window is not SceneView view)
                return;
            if (Event.current.type == EventType.Repaint)
                DrawGrid(MapEditorManager.activeTarget.grid);
        }

        protected override Type GetEditorToolType(Tool tool)
        {
            return null;
        }

      

        public void DrawGrid(Grid grid)
        {
            Handles.color = new Color(1f, 1f, 1f, 0.7f);
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            int gridSize = grid.gridSize;
            float tileSize = grid.cellSize;


            for (int x = -gridSize; x <= gridSize; x++)
            {
                Handles.DrawLine(
                    new Vector3(x * tileSize, 0, -gridSize * tileSize),
                    new Vector3(x * tileSize, 0, gridSize * tileSize)
                );
            }

            for (int z = -gridSize; z <= gridSize; z++)
            {
                Handles.DrawLine(
                    new Vector3(-gridSize * tileSize, 0, z * tileSize),
                    new Vector3(gridSize * tileSize, 0, z * tileSize)
                );
            }
            Handles.color = Color.forestGreen;
            Handles.DrawSolidDisc(Vector3.zero, Vector3.up, 0.1f);
        }

    }

  
}