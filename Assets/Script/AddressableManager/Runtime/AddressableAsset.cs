namespace NgocDev.Core.Addressable
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AddressableAsset : IKeyEvaluator
    {
        [SerializeField]
        private string address;
        public object RuntimeKey => address;
        public AddressableAsset(string address)
        {
       
            this.address = address;
        }

        public bool RuntimeKeyIsValid()
        {           
            return !string.IsNullOrEmpty(address);
        }
    }
}