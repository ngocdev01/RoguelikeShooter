using NgocDev.Editor;
using NgocDev.Editor.Elements;
using NgocDev.Gameplay.MapGeneration;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.CM.Common.CmCallContext;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;
namespace NgocDev.Gameplay.MapGeneration.Editor.MapEditor
{
    public class MapEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private Map _targetMap => MapEditorManager.activeTarget;
        private TabView _tabView;
        private Tab _settingTab;
        private Tab _tilePaletteTab;
        private InspectorElement _settingInspector;

        [MenuItem("NgocDev/Map Editor")]
        public static void ShowWindow()
        {
            MapEditorWindow wnd = GetWindow<MapEditorWindow>();
            var icon = EditorGUIUtility.IconContent("PreMatCube").image;
            wnd.titleContent = new GUIContent("Map Editor", icon);
        }

        public static void CloseWindow()
        {
            var window = GetWindow<MapEditorWindow>();
            window?.Close();
        }

        private void OnEnable()
        {
            MapEditorManager.OpenMapEditor(_targetMap);
        }

        private void OnDisable()
        {
            MapEditorManager.CloseMapEditor();
        }

        public void CreateGUI()
        {
                
            VisualElement root = rootVisualElement;
            m_VisualTreeAsset.CloneTree(root);
            _tabView = root.Q<TabView>();
            _settingTab = _tabView.GetTab(0);
            _tilePaletteTab = _tabView.GetTab(1);
            _settingTab.selected += OnSettingSelect;
            _tilePaletteTab.selected += OnTilePaletteSelect;

        }

        private void OnSettingSelect(Tab tab)
        {
            var serializedObject = new SerializedObject(MapEditorSetting.instance);
            _settingInspector = new InspectorElement(serializedObject);
            tab.Clear();
            tab.Add(_settingInspector);
        }

        private void OnTilePaletteSelect(Tab tab)
        {
            tab.Clear();
            GridListView gridListView = new GridListView();
            gridListView.itemSource = MapEditorManager.activeTarget.tileMap.tilePrefabs;
            gridListView.makeItem = () =>
            {
                return new TileElement();
            };
            gridListView.bindItem = (element, index) =>
            {
                TileElement tileElement = element as TileElement;
                tileElement.SetValueWithoutNotify(MapEditorManager.activeTarget.tileMap.tilePrefabs[index]);
            };

            gridListView.onSelectionChanged += OnSelectionChanged;
            tab.Add(gridListView);

         
        }

        private void OnSelectionChanged(List<object> list)
        {
            foreach( var item in list)
            {
                if (item is TilePrefab tilePrefab)
                {
                    MapEditorManager.activeTile = tilePrefab;                
                    break;
                }
            }
        }
    }

}