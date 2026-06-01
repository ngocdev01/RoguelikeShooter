using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NgocDev.Gameplay.Effect
{
    public enum ComparisonOperator
    {
        [InspectorName(">")]
        GreaterThan,
        [InspectorName("<")]
        LessThan,
        [InspectorName("=")]
        EqualTo,
        [InspectorName("!=")]
        NotEqualTo,
        [InspectorName(">=")]
        GreaterThanOrEqualTo,
        [InspectorName("<=")]
        LessThanOrEqualTo
    }

    public static class StatExtension
    {
        public static bool Compare(this ComparisonOperator comparisonOperator, float value1, float value2)
        {
            return comparisonOperator switch
            {
                ComparisonOperator.GreaterThan => value1 > value2,
                ComparisonOperator.LessThan => value1 < value2,
                ComparisonOperator.EqualTo => value1 == value2,
                ComparisonOperator.NotEqualTo => value1 != value2,
                ComparisonOperator.GreaterThanOrEqualTo => value1 >= value2,
                ComparisonOperator.LessThanOrEqualTo => value1 <= value2,
                _ => throw new NotImplementedException()
            };
        }
    }

    [Serializable]
    public abstract class AbilityCondition
    {
        public event Action onTriggered;
        public abstract bool IsMet(EffectController target);
        public virtual void OnApply(EffectController target) { }
        public virtual void OnRemove(EffectController target) { }
    }

    public enum BooleanOperator
    {
        And,
        Or
    }

    [Serializable]
    public class AbilityConditionList
    {
        [SerializeReference]
        private List<AbilityCondition> conditions = new List<AbilityCondition>();
        private EffectController target;
        public void OnApply(EffectController target)
        {
            this.target = target;
            foreach (AbilityCondition condition in conditions)
            {
                condition.OnApply(target);
                condition.onTriggered += Evaluate;
            }
        }


        private void Evaluate()
        {
            foreach (AbilityCondition condition in conditions)
            {
                if (!condition.IsMet(target))
                {
                    return;
                }
            }
            onTriggered?.Invoke();
        }


        public void OnRemove(EffectController target)
        {
            foreach (AbilityCondition condition in conditions)
            {
                condition.OnRemove(target);
            }
        }

        public event Action onTriggered;
    }
}
