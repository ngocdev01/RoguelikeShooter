using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace NgocDev.Editor
{

    public abstract class ItemPickerProvider<TItem>
    {

        public virtual Texture2D FetchThumbnail(TItem item) => null;
        public virtual Texture2D FetchThumbnail(SearchItem searchItem, SearchContext context, TItem item) => null;
        public virtual string FetchLabel(TItem item) => item?.ToString();
        public virtual string FetchDescription(TItem item) => null;
        public virtual Texture2D FetchPreview(TItem item) => null;
        public virtual bool Filter(TItem item,SearchContext context)
        {
            var query = context.searchQuery ?? string.Empty;
            var label = FetchLabel(item) ?? GetItemId(item);
            if (string.IsNullOrEmpty(label))
                return false;
            return label.Contains(query, StringComparison.OrdinalIgnoreCase);
        }
        public abstract string GetItemId(TItem item);
        public abstract string GetFilterID();
        public virtual SearchItem CreateItem(SearchProvider provider, TItem item)
        {
            var id = GetItemId(item);
            var label = FetchLabel(item) ?? id;
            var description = FetchDescription(item);
            var thumbnail = FetchThumbnail(item);
            return provider.CreateItem(id, label, description, thumbnail, item);
        }

        public virtual IEnumerable<SearchItem> FetchItems(SearchContext context, List<SearchItem> list, SearchProvider provider, IReadOnlyList<TItem> allItems)
        {
            if (context.empty)
            {
                yield break;
            }
          


            foreach (var item in allItems)
            {
                var label = FetchLabel(item) ?? GetItemId(item);
                if (!Filter(item, context))
                    continue;
                var sItem = CreateItem(provider, item);
                yield return sItem;
            }
            yield break;


        }
    }

    public class ItemPicker<TItem> : IDisposable
    {
        private readonly SearchProvider _searchProvider;
        private readonly ItemPickerProvider<TItem> _itemPickerProvider;
        private readonly IReadOnlyList<TItem> _itemSource;
        private static string itemTypeNameID => typeof(TItem).Name.ToLower();

        public ItemPicker(ItemPickerProvider<TItem> itemPickerProvider, IReadOnlyList<TItem> itemSource)
        {
            _itemSource = itemSource ?? throw new ArgumentNullException(nameof(itemSource));
            _itemPickerProvider = itemPickerProvider ?? throw new ArgumentNullException(nameof(itemPickerProvider));

            _searchProvider = new SearchProvider($"itemPicker.{itemTypeNameID}", $"Item Picker {itemTypeNameID}")
            {
                fetchItems = FetchItems,
                filterId = _itemPickerProvider.GetFilterID(),
                fetchThumbnail = (searchItem, context) => _itemPickerProvider.FetchThumbnail(searchItem, context, (TItem)searchItem.data),
                fetchDescription = (searchItem, context) => searchItem.description,
                fetchLabel = (searchItem, context) => searchItem.label ?? searchItem.id ?? string.Empty,
                fetchPreview = (searchItem, context, vector, options) => _itemPickerProvider.FetchPreview((TItem)searchItem.data),
            };

        }

        private  async Awaitable<Texture2D> FetchThumbailAsync(SearchItem searchItem,SearchContext context)
        {
            return null;
        }

        private IEnumerable<SearchItem> FetchItems(SearchContext context, List<SearchItem> list, SearchProvider provider)
        {
            return _itemPickerProvider.FetchItems(context, list, provider, _itemSource);
        }


        public void ShowPicker(Action<SearchItem, bool> onItemsSelected)
        {
            var context = SearchService.CreateContext(new[] { _searchProvider });

            SearchService.ShowPicker(context, onItemsSelected);
        }

        public void Dispose()
        {

        }
    }
}
