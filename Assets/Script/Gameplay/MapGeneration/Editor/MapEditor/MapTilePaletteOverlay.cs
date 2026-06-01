
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using NgocDev.Gameplay.MapGeneration;
using NgocDev.Editor.Elements;
using UnityEngine;
using NgocDev.Editor;

using System.Collections.Generic;

namespace NgocDev.Gameplay.MapGeneration.Editor
{
    [Overlay(displayName ="Test", defaultDisplay = true,defaultDockZone = DockZone.RightDynamicPanel)]
    public class MapTilePaletteOverlay : Overlay
    {
        private TileMap _tileMap;
        public TilePrefab ActivePrefab { get; private set; }
        public MapTilePaletteOverlay(TileMap tileMap)
        {
            _tileMap = tileMap;
         
        }
        public override VisualElement CreatePanelContent()
        {
           
            var root = new VisualElement();
            root.styleSheets.Add(CustomElement.LoadMainStyleSheet());
            GridListView gridListView = new GridListView();
            gridListView.itemSource = _tileMap.tilePrefabs;
            gridListView.makeItem = () =>
            {
                return new TileElement();
            };
            gridListView.bindItem = (element, index) =>
            {
                TileElement tileElement = element as TileElement;
                tileElement.SetValueWithoutNotify(_tileMap.tilePrefabs[index]);
            };

            gridListView.onSelectionChanged += OnSelectionChanged;
            root.Add(gridListView);
           
            return root;

        }
        
        private void OnSelectionChanged(List<object> list)
        {
            foreach (var item in list)
            {
                if (item is TilePrefab tilePrefab)
                {
                    MapEditorManager.activeTile = tilePrefab;
                    Debug.Log($"Selected Tile Prefab: {tilePrefab.editorPath}");

                    break;

                }
            }
        }
    }
}
