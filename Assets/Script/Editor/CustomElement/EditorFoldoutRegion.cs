using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Editor
{
    public class EditorFoldoutRegion : Foldout
    {
        public static readonly new string ussClassName = "editor-foldout-region";
        public EditorFoldoutRegion(string tittle) : base()
        {
            this.text = tittle;
            this.styleSheets.Add(CustomElement.LoadMainStyleSheet());
            this.viewDataKey = ussClassName + tittle;
            this.AddToClassList(ussClassName);
        }
    }
}