namespace NgocDev.Editor.Elements
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;


    [UxmlElement]
    public partial class LoadTest : VisualElement
    {
        public static readonly string ussClassName = "load-test";
        public LoadTest()
        {
            AddToClassList(ussClassName);
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Script/Editor/CustomElement/CustomElements.uss"));
            VisualElement conveyor = new VisualElement();
            conveyor.AddToClassList("conveyor");
            Add(conveyor);

            VisualElement arrow = new VisualElement();
            arrow.AddToClassList("arrow");
            conveyor.Add(arrow);

           

        }
    }
}