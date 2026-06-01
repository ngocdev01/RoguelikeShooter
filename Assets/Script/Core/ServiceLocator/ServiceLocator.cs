using System;
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Core.ServiceLocator
{
    public interface IService
    {
        
    }


    public abstract class MonoService : MonoBehaviour , IService
    {
      
    }



    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InitializeAfterAttribute : Attribute
    {
        public InitializeAfterAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InitializeBeforeAttribute : Attribute
    {
        public InitializeBeforeAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
        public Type ServiceType { get; }
    }

    public class ServiceLocator
    {
        public static Dictionary<Type, IService> Services { get; private set; } = new Dictionary<Type, IService>();
        public static void RegisterService<T>(T service) where T : IService
        {
            var type = typeof(T);
            if (Services.ContainsKey(type))
            {
                throw new Exception($"Service of type {type} is already registered.");
            }
            Services[type] = service as IService;
        }

        public static void RegisterService(Type type, object service)
        {
            if (Services.ContainsKey(type))
            {
                throw new Exception($"Service of type {type} is already registered.");
            }
            Services[type] = service as IService;
        }

        public static T GetService<T>() where T : IService
        {
            var type = typeof(T);
            if (Services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            throw new Exception($"Service of type {type} is not registered.");
        }
        public static void UnregisterService<T>() where T : IService
        {
            var type = typeof(T);
            if (!Services.ContainsKey(type))
            {
                throw new Exception($"Service of type {type} is not registered.");
            }
            Services.Remove(type);
        }
        public static void ClearServices()
        {
            Services.Clear();
        }


    }
}