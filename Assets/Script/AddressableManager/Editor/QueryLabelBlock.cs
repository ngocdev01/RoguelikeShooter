using UnityEditor.AddressableAssets;
using UnityEditor.Search;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NgocDev.Core.Addressable.Editor
{
    [QueryListBlock("Addressable Label", "Addressable Label", "al")]
    public class QueryLabelBlock : QueryListBlock
    {

        public QueryLabelBlock(IQuerySource source, string id, string value, QueryListBlockAttribute attr)
            : base(source, id, value, attr)
        {

        }



        public override IEnumerable<SearchProposition> GetPropositions(SearchPropositionFlags flags)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var labels = settings.GetLabels();

            foreach (var label in labels)
            {
                yield return new SearchProposition(
                category: null,
                label: label,
                replacement: label.Replace(" ", ""),
                help: $"Assets with label: {label}",
                moveCursor: TextCursorPlacement.MoveAutoComplete);
            }
        }
    }
}