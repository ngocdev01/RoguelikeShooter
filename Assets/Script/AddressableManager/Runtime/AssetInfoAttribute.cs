using System;
using UnityEngine;


namespace NgocDev.Addressable
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssetInfoAttribute : PropertyAttribute
    {
        public string label;
        public string group;
        public System.Type type;

        public AssetInfoAttribute(System.Type type = null, string group = null, string label = null)
        {
            this.label = label;
            this.group = group;
            this.type = type;
        }
    }
}