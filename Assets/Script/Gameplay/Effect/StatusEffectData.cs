using NgocDev.Gameplay.Stat;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.Effect
{


    [System.Serializable]
    public class ApplyEffectBehavior : AbilityBehavior
    {
        [SerializeField]
        private GameplayEffect effect;

        public override void Apply(EffectController target)
        {
            
        }
    }

    
}
