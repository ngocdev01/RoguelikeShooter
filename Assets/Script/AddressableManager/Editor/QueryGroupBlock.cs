using UnityEditor.AddressableAssets;
using UnityEditor.Search;
using System.Collections.Generic;

namespace NgocDev.Core.Addressable.Editor
{
    [QueryListBlock("Addressable Group", "Addressable Group", "ag")]
    public class QueryGroupBlock : QueryListBlock
    {
        public QueryGroupBlock(IQuerySource source, string id, string value, QueryListBlockAttribute attr)
            : base(source, id, value, attr)
        {

        }


        public override IEnumerable<SearchProposition> GetPropositions(SearchPropositionFlags flags)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var groups = settings.groups;
            foreach (var group in groups)
            {
                yield return new SearchProposition(
                category: null,
                label: group.Name,
                data: group.Name.Replace(" ", ""),
                replacement: "ag=" + group.Name.Replace(" ", ""),
                help: $"Assets in group: {group.Name}",
                moveCursor: TextCursorPlacement.MoveAutoComplete);
            }
        }
    }
}