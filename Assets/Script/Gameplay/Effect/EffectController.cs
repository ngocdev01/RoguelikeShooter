using System;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.Effect
{
    [DisallowMultipleComponent]
    public class EffectController : MonoBehaviour
    {
        [SerializeReference]
        private List<ActiveEffect> effects = new List<ActiveEffect>();


        public void ApplyEffect(ActiveEffect effect)
        {
            if (effect == null) return;
            effects.Add(effect);
        }

        public void Update()
        {
            
        }

        public void RemoveEffect(ActiveEffect effect)
        {
       
        }

    }
}
