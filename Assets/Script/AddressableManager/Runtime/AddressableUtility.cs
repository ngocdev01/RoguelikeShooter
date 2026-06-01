using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Runtime.CompilerServices;
using System.Collections.Generic;





#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;


namespace NgocDev.Core.Addressable
{
    public class AddressableUtility
    {
        public static AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        private static List<string> labels;
        private void UpdateLabels()
        {
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings is null. Make sure Addressables are set up");
                return;
            }
            labels = settings.GetLabels();
        }

        

    }
}
#endif