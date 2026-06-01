
using NgocDev.Gameplay.Effect;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Gameplay.Stat.Editor
{
    [CustomPropertyDrawer(typeof(StatCondition))]
    public class StatConditionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexWrap = Wrap.Wrap,
                }
            };

            var statTypeProp = property.FindPropertyRelative("statType");
            var comparisonOperatorProp = property.FindPropertyRelative("comparisonOperator");
            var valueProp = property.FindPropertyRelative("value");

            var statTypeField = new ObjectField
            {
                objectType = typeof(StatDefinition),
                style = { flexGrow = 1 }
            };

            statTypeField.BindProperty(statTypeProp);

            var comparisonOperatorField = new EnumField();
            comparisonOperatorField.BindProperty(comparisonOperatorProp);

            var valueField = new FloatField
            {
                style = { flexGrow = 1 }
            };
            valueField.BindProperty(valueProp);

            root.Add(statTypeField);
            root.Add(comparisonOperatorField);
            root.Add(valueField);

            return root;
        }
    }
}
