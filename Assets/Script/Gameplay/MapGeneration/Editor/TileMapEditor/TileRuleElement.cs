
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



namespace NgocDev.Gameplay.MapGeneration.Editor
{
    [UxmlElement]
    public partial class TileRuleElement : BindableElement, INotifyValueChanged<TileRule>
    {
        private VisualTreeAsset _tileRuleUXML = default;

        public static readonly string ussClassName = "tilerule-element";
        public static readonly string slotUssClassName = ussClassName + "__slot";
        public static readonly string slotCenterUssClassName = ussClassName + "__center";


        private TileRule _value;
        public TileRule value { get => _value; set => SetValueNotify(value); }

        private List<NeighborTileElement> _neighbors = new List<NeighborTileElement>();
        


        private VisualElement _center;

        public TileRuleElement()
        {
            AddToClassList(ussClassName);
            _tileRuleUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Script/GamePlay/MapGeneration/Editor/TileRuleElement.uxml");
          
            if (_tileRuleUXML != null)
            {
          
                _tileRuleUXML.CloneTree(this);
            }
            this.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        private void SetValueNotify(TileRule newValue)
        {
            Debug.Log(newValue);
            
            var oldValue = _value;
            SetValueWithoutNotify(newValue);
            using (var evt = ChangeEvent<TileRule>.GetPooled(oldValue, newValue))
            {
                evt.target = this;
                SendEvent(evt);
            }

        }
        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _center = this.Q<VisualElement>(className: slotCenterUssClassName);
            _neighbors = this.Query<NeighborTileElement>().ToList();
            foreach (var neighbor in _neighbors)
            {
                neighbor.RegisterCallback<MouseDownEvent>(e =>
                {
                    Debug.Log(neighbor.direction);
                });
            }
        }
        private void RefreshView()
        {
           
        }
        public void SetValueWithoutNotify(TileRule newValue)
        {
            if (_value == newValue)
                return;
            _value = newValue;
            RefreshView();
        }

       
    }
    
  
}

