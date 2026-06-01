using NgocDev.Gameplay.Stat;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace NgocDev.Gameplay.Effect
{
    [Serializable]
    public abstract class EffectComponent
    {
        public virtual void OnApply(EffectController target) { }
        public virtual void OnUpdate(EffectController target) { }
        public virtual void OnRemove(EffectController target) { }
    }

    [Serializable]
    public class GameplayEffect
    {
        public EffectApplyPolicy stackPolicy = new ReplacePolicy();
        [SerializeReference]
        public List<EffectComponent> effectComponents = new List<EffectComponent>();
    }


    [Serializable]
    public class DoTEffect : EffectComponent
    {
        public float damagePerSecond;
        private float elapsedTime;
        public override void OnUpdate(EffectController target)
        {
            
        }
    }

    [Serializable]
    public class EffectVFXCompoent : EffectComponent
    {
        public GameObject vfxPrefab;
        private GameObject spawnedVFX;
        public override void OnApply(EffectController target)
        {
            if (vfxPrefab != null)
            {
                spawnedVFX = GameObject.Instantiate(vfxPrefab, target.transform);
            }
        }
        public override void OnRemove(EffectController target)
        {
            if (spawnedVFX != null)
            {
                GameObject.Destroy(spawnedVFX);
            }
        }
    }

    [Serializable]
    public class StatModifierEffect : EffectComponent
    {
        public StatDefinition statType;
        public float value;
        public StatModiferType modiferType;
        private StatModifier modifier;
        public override void OnApply(EffectController target)
        {
            modifier = new StatModifier(value, modiferType);
            target.GetComponent<Character.Character>().stats.GetStat(statType).AddModifier(modifier);
        }
        public override void OnRemove(EffectController target)
        {
            target.GetComponent<Character.Character>().stats.GetStat(statType).RemoveModifier(modifier);

        }
    }

    [Serializable]
    public class ActiveEffect
    {
        public object source;
        public GameplayEffect effect;
        public int stackCount;
        public List<float> stackDuration;
        public int maxStack;
    }
}

