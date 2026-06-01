using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.Stat
{
    [System.Serializable]
    public struct RangeFloat
    {
        public float min;
        public float max;
        public RangeFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        public float Clamp(float value) => Mathf.Clamp(value, min, max);
        public float Magnitude() => max - min;
    }

    public enum StatModiferType
    {
        Base, Percent, Flat
    }
    public class Stat
    {
        private float _value;
        private float _baseValue;
        private bool _isDirty;
        private List<StatModifier> _modifiers;
        private RangeFloat _valueRange;

        public event Action<StatModifier> ModifierChanged;
        public event Action<float> BaseValueChanged;
        public event Action<float> ValueChanged;

        public float Value
        {
            get
            {
                if (!_isDirty)
                {
                    return _value;
                }
                float percentSum = 0;
                float baseValue = _baseValue;
                float flatSum = 0;

                foreach (var modifier in _modifiers)
                {

                    switch (modifier.type)
                    {
                        case StatModiferType.Base:
                            baseValue += modifier.value;
                            break;
                        case StatModiferType.Percent:
                            percentSum += modifier.value;
                            break;
                        case StatModiferType.Flat:
                            flatSum += modifier.value;
                            break;
                    }
                }
                _value = baseValue * (1 + percentSum / 100f) + flatSum;
                _isDirty = false;
                ValueChanged?.Invoke(_value);
                return _value;
            }
        }

        public float ClampedValue => _valueRange.Clamp(Value);


        public Stat(float baseValue, RangeFloat? valueRange = null)
        {
            _baseValue = baseValue;
            _valueRange = valueRange ?? new RangeFloat(0.0f, float.MaxValue);
            _modifiers = new List<StatModifier>();
            _isDirty = true;
            
        }

        public float BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                _isDirty = true;
                BaseValueChanged?.Invoke(_baseValue);
            }
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            _isDirty = true;
            ModifierChanged?.Invoke(modifier);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (_modifiers.Remove(modifier))
            {
                _isDirty = true;
                ModifierChanged?.Invoke(modifier);
            }
        }
    }

    public class DynamicStat 
    {
        private Stat _baseStat;


    }


}