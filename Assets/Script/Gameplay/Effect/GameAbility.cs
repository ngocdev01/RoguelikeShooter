using System;
using UnityEngine;

namespace NgocDev.Gameplay.Effect
{
    [Serializable]
    public class GameAbility
    {
        [SerializeField]
        private AbilityConditionList conditionList;
        [SerializeField]
        private AbilityBehavior effectBehavior;
    }
}

