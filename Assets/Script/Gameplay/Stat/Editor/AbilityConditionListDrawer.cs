
using NgocDev.Gameplay.Effect;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Gameplay.Stat
{
    [CustomPropertyDrawer(typeof(AbilityConditionList))]
    internal class AbilityConditionListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var itemsProp = property.FindPropertyRelative("conditions");
            

            var listView = new ListView
            {             
                allowAdd = true,
                showAddRemoveFooter = true,
                reorderable = true,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            };

        

            listView.BindProperty(itemsProp);
            listView.showBoundCollectionSize = false;
            listView.showBorder = true;
            listView.headerTitle = "Effect Conditions";
            listView.showFoldoutHeader = true;

            listView.overridingAddButtonBehavior = (listView,button) => ShowTypePickerPopup(listView, button, itemsProp);

            root.Add(listView);
            return root;
        }

        private void ShowTypePickerPopup(BaseListView listView, VisualElement target, SerializedProperty itemsProp)
        {
            var menu = new GenericDropdownMenu();

            var types = TypeCache.GetTypesDerivedFrom<AbilityCondition>()
                                .Where(t => !t.IsAbstract && !t.IsInterface)
                                .OrderBy(t => t.Name);

            foreach (var type in types)
            {
                string displayName = ObjectNames.NicifyVariableName(type.Name);

                menu.AddItem(displayName, false, () =>
                {
                    int newIndex = itemsProp.arraySize;
                    itemsProp.InsertArrayElementAtIndex(newIndex);
                    itemsProp.GetArrayElementAtIndex(newIndex).managedReferenceValue =
                        Activator.CreateInstance(type);
                    itemsProp.serializedObject.ApplyModifiedProperties();
                    listView.RefreshItems();
                });
            }

            menu.DropDown(target.worldBound, target, DropdownMenuSizeMode.Auto);
        }
    }
}