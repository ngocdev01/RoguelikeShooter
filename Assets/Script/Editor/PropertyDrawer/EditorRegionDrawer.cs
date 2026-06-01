using UnityEditor;
using UnityEngine.UIElements;
using NgocDev.Core;

namespace NgocDev.Editor
{
    [CustomPropertyDrawer(typeof(EditorRegionAttribute))]
    public class EditorRegionDrawer : DecoratorDrawer
    {
        public override VisualElement CreatePropertyGUI()
        {
            var regionAttribute = attribute as EditorRegionAttribute;
            if (regionAttribute != null)
            {
                var foldoutRegion = new EditorFoldoutRegion(regionAttribute.RegionName);
                return foldoutRegion;
            }
            
            return new VisualElement();
        }
    }
}