
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using NgocDev.Gameplay.MapGeneration;
using NgocDev.Gameplay.MapGeneration.Editor;

namespace NgocDev.MapGeneration.Editor
{
    public class TileMapEditor : UnityEditor.EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset _tileMapEditorUXML = default;


        [SerializeField]
        private TileMap _tileMap;
        private ObjectField _tileMapField;
        private TileContainer _tileContainer;
        private TileRuleElement _tileRuleElement;
        private SerializedObject _serializedTileMap;

        [MenuItem("NgocDev/Tile Map")]
        public static void ShowTileMapEditor()
        {
            GetWindow<TileMapEditor>("Tile Map Editor").Show();
        }
        private void OnEnable()
        {
            
        }
    


        private void CreateGUI()
        {
            if (_tileMapEditorUXML != null)
            {
                _tileMapEditorUXML.CloneTree(rootVisualElement);
                _tileMapField = rootVisualElement.Q<ObjectField>();
                if (_tileMap != null)
                {
                    _tileContainer.SetTileMap(_tileMap);
                    _tileMapField.value = _tileMap;
                }
                _tileRuleElement = rootVisualElement.Q<TileRuleElement>();

                _tileContainer = rootVisualElement.Q<TileContainer>();
                _tileContainer.onSelectionIndicesChanged += (indices) =>
                {
                    var index = indices.First();
                    _tileRuleElement.value = _tileMap.tilePrefabs[index].tileRule;
                };

                _tileMapField.RegisterValueChangedCallback(evt =>
                {
                    _tileMap = evt.newValue as TileMap;
                    EditorUtility.SetDirty(this);
                    _serializedTileMap = new SerializedObject(_tileMap);
                    _tileContainer.SetTileMap(_tileMap);

                });



            }
        }






    }




}









