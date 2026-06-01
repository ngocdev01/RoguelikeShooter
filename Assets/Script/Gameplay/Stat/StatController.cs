using System;
using System.Collections.Generic;
using System.Text;

namespace NgocDev.Gameplay.Stat
{
    public class StatController
    {
        private Dictionary<StatDefinition, Stat> _stats = new Dictionary<StatDefinition, Stat>();


        public Stat GetStat(StatDefinition definition)
        {
            if (_stats.TryGetValue(definition, out var stat))
            {
                return stat;
            }
            return null;
        }

        public void AddModifier(StatDefinition statType, StatModifier modifier)
        {
            var stat = GetStat(statType);
            if (stat != null)
            {
                stat.AddModifier(modifier);
            }
        }
        public void RemoveModifier(StatDefinition statType, StatModifier modifier)
        {
            var stat = GetStat(statType);
            if (stat != null)
            {
                stat.RemoveModifier(modifier);
            }
        }
    }
}
