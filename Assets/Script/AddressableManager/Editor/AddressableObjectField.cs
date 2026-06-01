


using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Search;
using UnityEngine.UIElements;


namespace NgocDev.Addressable.Editor
{

    public class AddressableObjectField : BindableElement, INotifyValueChanged<string>
    {
        private string _value;
        private UnityEditor.Search.ObjectField _objectField;
        private string group;
        private string label;
        private Type objectType;

        public string value { get => _value; set => SetValueNotify(value); }

        private void SetValueNotify(string newValue)
        {
            if (newValue == _value) return;
            var oldValue = _value;
            SetValueWithoutNotify(newValue);
            using (var evt = ChangeEvent<string>.GetPooled(oldValue, value))
            {
                evt.target = this;
                SendEvent(evt);
            }
        }

        public AddressableObjectField(string label = null)
        {

            _objectField = new UnityEditor.Search.ObjectField(label);
            _objectField.objectType = objectType ?? typeof(UnityEngine.Object);
       
            _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(_objectField);
        }

        private void OnObjectFieldValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            string path = AssetDatabase.GetAssetPath(evt.newValue);
            string guid = AssetDatabase.AssetPathToGUID(path);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);
            value = entry != null ? entry.address : string.Empty;
        }

        public void SetValueWithoutNotify(string newValue)
        {
            

            _value = newValue;
            if (string.IsNullOrEmpty(_value))
            {
                _objectField.SetValueWithoutNotify(null);
               
                return;
            }
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) return;

            foreach (var group in settings.groups)
            {
                var entry = group.entries.FirstOrDefault(e => e.address == _value);
                if (entry != null)
                {
                    _objectField.SetValueWithoutNotify(entry.TargetAsset);
                    return;
                }
            }
            _objectField.SetValueWithoutNotify(null);


        }
    }
}
