using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Search;


namespace NgocDev.Editor.Elements
{
    [UxmlElement]
    public partial class GridListView : VisualElement
    {
        public static readonly string ussClassName = "grid-list-view";
        public static readonly string scrollViewUssClassName = ussClassName + "__scrollview";
        public static readonly string containerUssClassName = ussClassName + "__container";
        public static readonly string itemUssClassName = ussClassName + "__item";

        internal class GridListViewItem : VisualElement
        {
            public static readonly string ussClassName = GridListView.itemUssClassName;
            public static readonly string selectedUssClassName = ussClassName + "--selected";

            public int index { get; set; }
            public int id { get; set; }

            public VisualElement rootElement { get; protected set; }

            public GridListViewItem()
            {
                AddToClassList(ussClassName);
                AddToClassList(selectedUssClassName);
                SetSelected(false);
            }

            public void Init(VisualElement root, float width, float height)
            {
                this.rootElement = root;
                rootElement.style.width = width;
                rootElement.style.height = height;
                Add(rootElement);
            }

            public void SetSelected(bool selected)
            {
                if (selected)
                {
                    EnableInClassList(selectedUssClassName, true);
                }
                else
                {
                    EnableInClassList(selectedUssClassName, false);
                }
            }
        }



        private HashSet<int> _selectedIndices = new HashSet<int>();
        private List<object> _selectedItems = new List<object>();

        private ScrollView _scrollView;
        private int _lastSelectedIndex = -1;
        private IList _itemSource;
        private Func<VisualElement> _makeItem;
        private Action<VisualElement, int> _bindItem;
        private List<VisualElement> _itemElements;

        private float _itemHeight = 100f;
        private float _itemWidth = 100f;


        public Action<List<object>> onSelectionChanged;
        public Action<List<int>> onSelectedIndicesChanged;

        public float itemHeight
        {
            get => _itemHeight;
            set
            {
                if (_itemHeight != value)
                {
                    _itemHeight = value;
                    RefreshView();
                }
            }
        }

        public float itemWidth
        {
            get => _itemWidth;
            set
            {
                if (_itemWidth != value)
                {
                    _itemWidth = value;
                    RefreshView();
                }
            }
        }


        public override VisualElement contentContainer => _scrollView.contentContainer;
        public IList itemSource
        {
            get => _itemSource;
            set
            {
                if (_itemSource is INotifyCollectionChanged collection)
                {
                    collection.CollectionChanged -= OnItemSourceCollectionChanged;
                }
                _itemSource = value;

                if (_itemSource is INotifyCollectionChanged newCollection)
                {

                    newCollection.CollectionChanged += OnItemSourceCollectionChanged;
                }

                RefreshView();
            }
        }

        private void OnItemSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshView();
        }

        public Func<VisualElement> makeItem
        {
            get => _makeItem; set
            {
                if (_makeItem != value)
                {
                    _makeItem = value;
                    RefreshView();

                }
            }
        }



        public Action<VisualElement, int> bindItem
        {
            get => _bindItem;
            set
            {
                if (_bindItem != value)
                {
                    _bindItem = value;
                    RefreshView();
                }
            }
        }

        public IReadOnlyCollection<int> selectedIndices => _selectedIndices;

        public GridListView()
        {

            AddToClassList(ussClassName);
            _scrollView = new ScrollView();
            hierarchy.Add(_scrollView);
            _scrollView.AddToClassList(scrollViewUssClassName);
            contentContainer.AddToClassList(containerUssClassName);
            _scrollView.RegisterCallback<AttachToPanelEvent>(OnScrollViewAttach);
            _scrollView.verticalScroller.valueChanged += OnScroll;

            _bindItem = BindItem;
            _makeItem = MakeItem;
        }

        private void OnScroll(float obj)
        {

        }

        private void OnScrollViewAttach(AttachToPanelEvent evt)
        {
            _scrollView.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _scrollView.RegisterCallback<KeyDownEvent>(OnKeyDown);
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if ((evt.ctrlKey || evt.commandKey) && evt.keyCode == KeyCode.A)
            {
                SelectAll();
                evt.StopPropagation();
            }
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            int index = GetItemIndexAtPosition(evt.localPosition);

            if (index == -1 || index >= _itemElements.Count)
                return;

            var itemElement = _itemElements[index] as GridListViewItem;
            if (itemElement == null)
                return;

            bool isSelected = _selectedIndices.Contains(index);

            if (evt.shiftKey && _lastSelectedIndex >= 0)
            {
                int start = Mathf.Min(_lastSelectedIndex, index);
                int end = Mathf.Max(_lastSelectedIndex, index);

                for (int i = start; i <= end; i++)
                {
                    if (!_selectedIndices.Contains(i))
                    {
                        _selectedIndices.Add(i);
                        _selectedItems.Add(_itemSource[i]);
                        if (_itemElements[i] is GridListViewItem item)
                        {
                            item.SetSelected(true);
                        }
                    }
                }
                evt.StopPropagation();
                return;
            }


            if (evt.ctrlKey || evt.commandKey)
            {
                if (isSelected)
                {
                    _selectedIndices.Remove(index);
                    _selectedItems.Remove(_itemSource[index]);
                    itemElement.SetSelected(false);
                }
                else
                {
                    _selectedIndices.Add(index);
                    _selectedItems.Add(_itemSource[index]);
                    itemElement.SetSelected(true);
                }
                _lastSelectedIndex = index;
                evt.StopPropagation();
                return;
            }


            ClearSelection();
            _selectedIndices.Add(index);
            _selectedItems.Add(_itemSource[index]);
            itemElement.SetSelected(true);
            _lastSelectedIndex = index;
            evt.StopPropagation();
            NotifySelectionChanged();
        }

        private int GetItemIndexAtPosition(Vector2 position)
        {
            float x = position.x;
            float y = position.y + _scrollView.scrollOffset.y;
            int itemsPerRow = Mathf.FloorToInt(contentContainer.layout.width / _itemWidth);
            if (itemsPerRow <= 0) itemsPerRow = 1;
            int row = Mathf.FloorToInt(y / _itemHeight);
            int column = Mathf.FloorToInt(x / _itemWidth);
            int index = row * itemsPerRow + column;
            if (index >= 0 && index < _itemSource.Count)
            {
                return index;
            }
            return -1;
        }

        private VisualElement MakeItem()
        {
            var item = new VisualElement();
            return item;
        }

        private void BindItem(VisualElement element, int index)
        {

        }


        public void RefreshView()
        {
            contentContainer.Clear();
            _itemElements = new List<VisualElement>();
            _lastSelectedIndex = -1;
            _selectedIndices.Clear();
            if (_itemSource != null)
            {
                for (int i = 0; i < _itemSource.Count; i++)
                {
                    var element = _makeItem?.Invoke();
                    var itemElement = new GridListViewItem();
                    itemElement.Init(element, _itemWidth, _itemHeight);
                    itemElement.index = i;
                    itemElement.AddToClassList(itemUssClassName);
                    contentContainer.Add(itemElement);
                    _bindItem?.Invoke(element, i);
                    _itemElements.Add(itemElement);
                }
            }
        }

        private void SelectAll()
        {
            _selectedIndices.Clear();
            _selectedItems.Clear();

            for (int i = 0; i < _itemElements.Count; i++)
            {
                _selectedIndices.Add(i);
                _selectedItems.Add(_itemSource[i]);
                if (_itemElements[i] is GridListViewItem item)
                {
                    item.SetSelected(true);
                }
            }
            _lastSelectedIndex = _itemElements.Count > 0 ? _itemElements.Count - 1 : -1;
            NotifySelectionChanged();
        }

        private void ClearSelection()
        {
            foreach (int index in _selectedIndices)
            {
                if (index >= 0 && index < _itemElements.Count && _itemElements[index] is GridListViewItem item)
                {
                    item.SetSelected(false);
                }
            }
            _selectedIndices.Clear();
            _selectedItems.Clear();
            _lastSelectedIndex = -1;
            
        }

        private void NotifySelectionChanged()
        {
            onSelectionChanged?.Invoke(_selectedItems);
            onSelectedIndicesChanged?.Invoke(new List<int>(_selectedIndices));
        }
    }
}