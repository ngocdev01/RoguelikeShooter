using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace NgocDev.UI
{
    public class FolderPicker : BindableElement, INotifyValueChanged<string>
    {
        public static readonly string ussClassName = "folder-picker";
        public static readonly string inputUssClassName = ussClassName + "__input";
        public static readonly string buttonUssClassName = ussClassName + "__button";

        private TextField _pathField;
        private Button _openFolderButton;

        public FolderPicker(string label) 
        {
            AddToClassList(ussClassName);

            
            _pathField = new TextField(label);
            _pathField.AddToClassList(inputUssClassName);
            _pathField.AddToClassList(BaseField<string>.alignedFieldUssClassName);
            _pathField.isReadOnly = true;
            Add(_pathField);
            

            _openFolderButton = new Button(OnOpenFolder);
            _openFolderButton.AddToClassList(buttonUssClassName);
            _openFolderButton.text = "...";
            _openFolderButton.style.width = 30;
            _pathField.Add(_openFolderButton);

            RegisterCallback<AttachToPanelEvent>(evt => 
            {
                if (bindingPath != null)
                {
                    _pathField.bindingPath = bindingPath;
                }
            });
        }

        public string value { get => _pathField.value; set => _pathField.value = value; }

        public void SetValueWithoutNotify(string newValue)
        {
            _pathField?.SetValueWithoutNotify(newValue);
        }

        private void OnOpenFolder()
        {
            string currentPath = string.IsNullOrEmpty(_pathField.value) ? Application.dataPath : _pathField.value;
         
            string selectedPath = UnityEditor.EditorUtility.OpenFolderPanel("Select Folder", currentPath, "");

            if (!string.IsNullOrEmpty(selectedPath))
            {
                
                if (selectedPath.StartsWith(Application.dataPath))
                {
                    selectedPath =  selectedPath.Substring(Application.dataPath.Length);
                }

                value = selectedPath;
            }
        }
    }
}