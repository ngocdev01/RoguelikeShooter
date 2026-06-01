using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Search;

namespace NgocDev.Gameplay.Effect
{
    [Serializable]
    public class EventCondition : AbilityCondition
    {
        private bool triggeredThisFrame = false;
        public Type eventType;

        public override bool IsMet(EffectController target)
        {
            if (triggeredThisFrame)
            {
                triggeredThisFrame = false;
                return true;
            }
            return false;
        }
    }
}
