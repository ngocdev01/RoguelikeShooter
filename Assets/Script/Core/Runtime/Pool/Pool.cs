using NgocDev.Core.ServiceLocator;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace NgocDev.Core.Pool
{
    public class PoolService : MonoService
    {
        Dictionary<Type, object> _pools = new Dictionary<Type, object>();
    }

    public class PoolRoot : MonoBehaviour
    {
        private Scene activeScene;
        private void Awake()
        {


        }

        private void OnDestroy()
        {

        }
    }

    [DisallowMultipleComponent]
    public class PooledAsset : MonoBehaviour
    {
        private Type _mainComponenType;
        private Component _mainComponent;
        private AssetPool _pool;
        public T GetMainComponent<T>() where T : Component
        {
            if (_mainComponenType == null)
            {
                _mainComponent = GetComponent<T>();
                _mainComponenType = typeof(T);
            }
            else if (_mainComponenType != typeof(T))
            {
                throw new Exception($"Requested component type {typeof(T)} does not match the main component type {_mainComponenType}.");
            }
            return _mainComponent as T;
        }

        public void Release()
        {
            _pool.ReleaseAsset(this);
        }
        public void SetPool(AssetPool pool)
        {
            _pool = pool;
        }


    }

    public class AssetPool 
    {
        private ObjectPool<PooledAsset> _pool;
        private Type _poolObjectType;
        private GameObject _assetHandle;
        private int _refCount;
        public AssetPool(string key,Type type)
        {
            _poolObjectType = type;
            _pool = new ObjectPool<PooledAsset>(CreateFunc, ActionOnGet, ActionOnRelease);
            _refCount = 0;
        }

        private void ActionOnRelease(PooledAsset obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void ActionOnGet(PooledAsset obj)
        {
            obj.gameObject.SetActive(true);

        }

        private PooledAsset CreateFunc()
        {
            var poolObject = GameObject.Instantiate(_assetHandle).AddComponent<PooledAsset>();
            poolObject.SetPool(this);
            return poolObject;
        }

        public async Awaitable Initialize(string key)
        {
            _assetHandle = await Addressables.LoadAssetAsync<GameObject>(key).Task;
        }
        public AssetPool Accquire()
        {
            _refCount++;
            return this;
        }

        public PooledAsset GetAsset()
        {
            return _pool.Get();
        }

        public void ReleaseAsset(PooledAsset asset)
        {
            _pool.Release(asset);
        }

        public void Release()
        {
            _refCount--;
            if (_refCount <= 0)
            {
               
            }
        }

        public void RelasePool()
        {
            _pool.Clear();
            Addressables.Release(_assetHandle);
            _assetHandle = null;
        }

        private void OnDestroy()
        {
            RelasePool();
        }

    }
}