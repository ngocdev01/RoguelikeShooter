using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Search;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using PlasticGui.WorkspaceWindow.Items;
using Unity.Profiling;

namespace NgocDev.Core.Addressable.Editor
{

    public static class AddressablesSearchProvider
    {
        internal static string id = "addr:";
        private static QueryEngine<AddressableAssetEntry> queryEngine;

       

        public static QueryEngine<AddressableAssetEntry> SetUpQueryEngine()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var queryEngine = new QueryEngine<AddressableAssetEntry>();
            queryEngine.AddFilter("al", entry => string.Join(",", entry.labels).Replace(" ", ""));
            queryEngine.AddFilter("ag", entry => entry.parentGroup.Name.Replace(" ", ""));
            queryEngine.SetSearchDataCallback(entry => new[] { entry.TargetAsset?.name });
            return queryEngine;
        }

        [SearchItemProvider]
        public static SearchProvider Create()
        {

            queryEngine = SetUpQueryEngine();
            return new SearchProvider(id, "Addressable Assets")
            {
                filterId = id,
                fetchItems = FetchItems,
                fetchThumbnail = FetchThumbnail,
                fetchPreview = FetchPreview,
                showDetailsOptions = ShowDetailsOptions.Inspector,
                fetchLabel = FetchLabel,
                fetchPropositions = FetchPropositions,
                trackSelection = TrackSelection,
                toObject = (item, context) => item.data as UnityEngine.Object,
     

            };
        }
        private static void TrackSelection(SearchItem searchItem, SearchContext searchContext)
        {
            GUID.TryParse(searchItem.id, out var guid);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetByGUID(guid, typeof(UnityEngine.Object)));
        }
        private static IEnumerable<SearchProposition> FetchPropositions(SearchContext context, SearchPropositionOptions options)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var labels = settings.GetLabels();

            foreach (var label in labels)
            {
                yield return new SearchProposition(
                    category: "Addressable Label",
                    label: label,
                    replacement: "al=" + label.Replace(" ", ""),
                    help: $"Assets with label: ",
                    moveCursor: TextCursorPlacement.MoveAutoComplete);
            }
            var groups = settings.groups;
            foreach (var group in groups)
            {
                yield return new SearchProposition(
                    category: "Addressable Group",
                    label: group.Name,
                    replacement: "ag=" + group.Name.Replace(" ", ""),
                    help: $"Assets in group: ",
                    moveCursor: TextCursorPlacement.MoveAutoComplete);
            }

        }

        private static Texture2D FetchPreview(SearchItem item, SearchContext context, Vector2 vector, FetchPreviewOptions options)
        {
            return AssetPreview.GetAssetPreview(item.data as UnityEngine.Object);
        }

        private static string FetchLabel(SearchItem item, SearchContext context)
        {
            var entry = item.data as AddressableAssetEntry;
            return entry != null ? entry.address : string.Empty;
        }

        private static Texture2D FetchThumbnail(SearchItem item, SearchContext context)
        {
            return EditorGUIUtility.ObjectContent(item.data as UnityEngine.Object, typeof(UnityEngine.Object)).image as Texture2D;
        }


        private static IEnumerable<SearchItem> FetchItems(SearchContext context, List<SearchItem> list, SearchProvider provider)
        {
            var query = queryEngine.ParseQuery(context.searchQuery);
            if (!query.valid || string.IsNullOrEmpty(query.text))
            {
                foreach (var error in query.errors)
                {
                    Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, $"Error parsing input at {error.index}: {error.reason}");
                }
                yield break;
            }
            var assetEntries = new List<AddressableAssetEntry>();
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.GetAllAssets(assetEntries, false);
            var result = query.Apply(assetEntries);
         
            if (result == null )
                yield break;
            foreach (var entry in result)
            {
                yield return provider.CreateItem(entry.guid, entry.TargetAsset?.name, entry.address, null, entry.TargetAsset);
            }
        }
    }
}
