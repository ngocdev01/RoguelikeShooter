using NgocDev.Gameplay.Stat;
using System;

namespace NgocDev.Gameplay.Effect
{

    public class PersistentCondition : AbilityCondition
    {
        public event Action onTriggered;
        public override bool IsMet(EffectController target) => true;


    }


    [Serializable]
    public class StatCondition : AbilityCondition
    {
        public StatDefinition statType;
        public ComparisonOperator comparisonOperator;
        public float value;
        private float cachedValue;
        private Func<float, bool> compareFunction;


        public override bool IsMet(EffectController target) => compareFunction(cachedValue);

        public override void OnApply(EffectController target)
        {
            compareFunction = comparisonOperator switch
            {
                ComparisonOperator.GreaterThan => v => v > this.value,
                ComparisonOperator.LessThan => v => v < this.value,
                ComparisonOperator.EqualTo => v => v == this.value,
                ComparisonOperator.NotEqualTo => v => v != this.value,
                ComparisonOperator.GreaterThanOrEqualTo => v => v >= this.value,
                ComparisonOperator.LessThanOrEqualTo => v => v <= this.value,
                _ => throw new NotImplementedException()
            };
        }

      
    }

      
}
