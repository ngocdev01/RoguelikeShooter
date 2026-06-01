using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Script.Core.Data
{
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField]
        private T _value;

        public virtual T value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
        public event Action<T> OnValueChanged;
    }
}
