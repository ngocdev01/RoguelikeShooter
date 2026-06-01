using System;
using UnityEngine;

namespace NgocDev.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EditorRegionAttribute : PropertyAttribute
    {
        public string RegionName { get; private set; }
        public EditorRegionAttribute(string regionName)
        {
            RegionName = regionName;
        }
    }
}