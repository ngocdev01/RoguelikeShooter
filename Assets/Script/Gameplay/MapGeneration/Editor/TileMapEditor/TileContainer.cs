
using NgocDev.Editor.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



namespace NgocDev.Gameplay.MapGeneration.Editor
{
  

    [UxmlElement]
    public partial class TileContainer : VisualElement
    {
        public static readonly string ussClassName = "tile-container";
        public static readonly string containerUssClassName = ussClassName + "__container";
        public static readonly string dragOverUssClassName = ussClassName + "--drag-over";
  
        
        private readonly GridListView _gridListView;
        private TileMap _tileMap;
        public event Action OnChange;
        private SerializedObject serializedMap;

        public Action<TilePrefab> onSelectionChanged;
        public Action<List<int>> onSelectionIndicesChanged;


        public override VisualElement contentContainer => _gridListView.contentContainer;
        private void BindGridView()
        {
         
            _gridListView.itemSource = _tileMap.tilePrefabs;
            _gridListView.makeItem = () => new TileElement();
            _gridListView.bindItem = (element, index) =>
            {
                if (element is TileElement tileElement)
                {
                    tileElement.SetValueWithoutNotify(_tileMap.tilePrefabs[index]);
                   
                }
            };
            _gridListView.RefreshView();
        }
        public TileContainer()
        {
            serializedMap = _tileMap ? new SerializedObject(_tileMap) : null;
            AddToClassList(ussClassName);

            _gridListView = new GridListView();
            _gridListView.onSelectionChanged += (selectedItem) =>
            {
                if (selectedItem.FirstOrDefault() is TilePrefab tilePrefab)
                {
                    onSelectionChanged?.Invoke(tilePrefab);
                }
            };

            _gridListView.onSelectedIndicesChanged += (selectedIndex) =>
            {
                onSelectionIndicesChanged?.Invoke(selectedIndex);
            };

            contentContainer.AddToClassList(containerUssClassName);
           
            hierarchy.Add(_gridListView); 

          
            AddToClassList(dragOverUssClassName);
            EnableInClassList(dragOverUssClassName, false);

            RegisterCallback<AttachToPanelEvent>(evt => RegisterCallbacks());
            RegisterCallback<DetachFromPanelEvent>(evt => UnregisterCallbacks());
        }

        
        public void SetTileMap(TileMap tileMap)
        {
            _tileMap = tileMap;
            serializedMap = new SerializedObject(_tileMap);
            BindGridView();
        }

        private void RegisterCallbacks()
        {
            RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
            RegisterCallback<DragLeaveEvent>(OnDragLeave);
            RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        private void UnregisterCallbacks()
        {
            UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
            UnregisterCallback<DragLeaveEvent>(OnDragLeave);
            UnregisterCallback<DragPerformEvent>(OnDragPerform);
        }

        private void OnDragUpdate(DragUpdatedEvent evt)
        {
            if (IsValidDrag())
            {
                EnableInClassList(dragOverUssClassName, true);
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            }
        }

        private void OnDragLeave(DragLeaveEvent evt)
        {
            EnableInClassList(dragOverUssClassName, false);
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            EnableInClassList(dragOverUssClassName, false);

            if (!IsValidDrag())
                return;

            DragAndDrop.AcceptDrag();

            foreach (var obj in DragAndDrop.objectReferences)
            {
                if (!IsValidObject(obj))
                    continue;

                string assetPath = AssetDatabase.GetAssetPath(obj);
               
                if (_tileMap.tilePrefabs.Any(t => t.editorPath == assetPath))
                {
                    Debug.Log($"Tile already added: {obj.name}");
                    continue;
                }

                TilePrefab newTilePrefab = new TilePrefab
                {
                    editorPath = assetPath,
                    prefab = assetPath,
                };

                _tileMap.tilePrefabs.Add(newTilePrefab);


            }

            AssetDatabase.Refresh();
        }
        private bool IsValidObject(UnityEngine.Object obj)
        {
            return PrefabUtility.IsPartOfPrefabAsset(obj) && AssetDatabase.IsMainAsset(obj);
                   
        }
        private bool IsValidDrag()
        {
            return DragAndDrop.objectReferences.Any(obj =>
                IsValidObject(obj));
        }
    }
}

