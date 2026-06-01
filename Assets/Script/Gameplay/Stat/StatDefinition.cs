using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NgocDev.Gameplay.Stat
{
    [CreateAssetMenu(fileName = "New Stat Definition", menuName = "Gameplay/Stat/Stat Definition")]
    public class StatDefinition : ScriptableObject
    {
        public string statName;
        [TextArea]
        public string description;
        public Texture2D icon;
    }

    

    public class StatModifier
    {
        public float value;
        public StatModiferType type;
        public StatModifier(float value, StatModiferType type)
        {
            this.value = value;
            this.type = type;
        }
    }
    



    public interface IEffectSource
    {
    }

 
}
