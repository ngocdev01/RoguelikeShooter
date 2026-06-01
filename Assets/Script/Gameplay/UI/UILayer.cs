
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NgocDev.Gameplay.UI
{
    public enum UILayerType
    {
        Background,
        Main,
        Popup,
        Overlay,
        Tooltip
    }

    public class UILayer
    {
        private Canvas _canvas;
        private Graphic _mainContainer;
        private int _sortingOrder;
        private UILayerType _layerType;
        public UILayer(Canvas canvas, Graphic mainContainer, int sortingOrder)
        {
            _canvas = canvas;
            _mainContainer = mainContainer;
            _sortingOrder = sortingOrder; 
        }
    }

    public class UIManager
    {
        private List<UILayer> _uiLayers = new List<UILayer>();
    }
}