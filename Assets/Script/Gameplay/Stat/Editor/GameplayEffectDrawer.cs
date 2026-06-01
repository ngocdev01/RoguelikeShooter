using NgocDev.Gameplay.Effect;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NgocDev.Gameplay.Stat.Editor
{
    [CustomPropertyDrawer(typeof(GameplayEffect))]
    public class GameplayEffectDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var stackPolicyProp = property.FindPropertyRelative("stackPolicy");
            var effectComponentsProp = property.FindPropertyRelative("effectComponents");

            var stackPolicyField = new PropertyField(stackPolicyProp);
            var listView = new ListView
            {
                allowAdd = true,
                showAddRemoveFooter = true,
                reorderable = true,
                showBoundCollectionSize = false,
                showBorder = true,
                showFoldoutHeader = false,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,

            };
          
         
            listView.overridingAddButtonBehavior = (view, button) => OnAdd(view, button, effectComponentsProp);
            listView.BindProperty(effectComponentsProp);
            root.Add(stackPolicyField);
            root.Add(listView);
            return root;

        }

        private void OnAdd(BaseListView view, Button button,SerializedProperty property)
        {
            var types = TypeCache.GetTypesDerivedFrom<EffectComponent>();
            var menu = new GenericDropdownMenu();
            foreach ( var type in types ) {
                menu.AddItem(type.Name, false, () =>
                {
                    var newElement = Activator.CreateInstance(type);
                    int newIndex = property.arraySize;
                    property.InsertArrayElementAtIndex(newIndex);
                    property.GetArrayElementAtIndex(newIndex).managedReferenceValue = newElement;
                    property.serializedObject.ApplyModifiedProperties();
                    view.RefreshItems();
                });
            }
            menu.DropDown(button.worldBound,button,DropdownMenuSizeMode.Content);
        }
    }
}
