using NgocDev.Gameplay.MapGeneration;
using NgocDev.Gameplay.MapGeneration.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Script.GamePlay.MapGeneration.Editor.Render
{
    public static class MapOpen
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {

            var asset = EditorUtility.EntityIdToObject(EntityId.FromULong((ulong)instanceID));
            if (asset is Map map)
            {
                MapEditorManager.OpenMapEditor(map);

            }

            return false;

        }
    }
}
