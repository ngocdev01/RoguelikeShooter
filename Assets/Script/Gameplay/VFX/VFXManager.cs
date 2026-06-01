using NgocDev.Core.ServiceLocator;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using System;

namespace NgocDev.Gameplay.VFX
{
    public class VFXManager : MonoService
    {
        private Dictionary<string, ObjectPool<VFXController>> _vfxPools;

        private void Awake()
        {
            _vfxPools = new Dictionary<string, ObjectPool<VFXController>>();
        }

        private ObjectPool<VFXController> CreatePool(string key)
        {
            return new ObjectPool<VFXController>(
                createFunc: () => null, // Replace with actual creation logic
                actionOnGet: (vfx) => vfx.Play(),
                actionOnRelease: (vfx) => vfx.Stop(),
                actionOnDestroy: (vfx) => Destroy(vfx.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 100
            );
        }
    }

    public class VFXPool : IDisposable
    {
        private VFXController _prefab;
        private ObjectPool<VFXController> _pool;
        

        public async Awaitable CreatePool(string key)
        {
            _prefab = await Addressables.LoadAssetAsync<VFXController>(key).Task;
            if (_prefab == null)
            {
                Debug.LogError($"Failed to load VFX prefab with key: {key}");
                return;
            }

            _pool = new ObjectPool<VFXController>(
                createFunc: CreateVFX,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroy,
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 100
            );


        }

        public void Dispose()
        {
            _pool.Dispose();
            Addressables.ReleaseInstance(_prefab.gameObject);
        }

        private VFXController CreateVFX() => GameObject.Instantiate(_prefab);
        private void OnGet(VFXController vfx) => vfx.gameObject.SetActive(true);
        private void OnRelease(VFXController vfx) => vfx.gameObject.SetActive(false);

        private void OnDestroy(VFXController vfx) => GameObject.Destroy(vfx.gameObject);




    }

    public abstract class VFXController : MonoBehaviour 
    {
        public virtual float PlaybackSpeed { get; set; }    
        public virtual void Play() { }
        public virtual void Stop() { }
       
    }

    public class  ParticleSystemVFXController : VFXController
    {
        private ParticleSystem _particleSystem;
        override public float PlaybackSpeed
        {
            get => _particleSystem.main.simulationSpeed;
            set
            {
                var mainModule = _particleSystem.main;
                mainModule.simulationSpeed = value;
            }                  
        }
        public override void Play() =>_particleSystem.Play();
        public override void Stop() => _particleSystem.Stop();


    }

}