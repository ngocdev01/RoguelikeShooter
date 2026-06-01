using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NgocDev.Core.StartUp
{
    public enum StartUpType
    {
        PreLoad,
        BeforeGameplay,
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class StartUpMethodAttribute: Attribute
    {
        public StartUpType Type { get; set; } = StartUpType.PreLoad;
        public StartUpMethodAttribute(StartUpType startUpType = StartUpType.PreLoad)
        {
            Type = startUpType; // Fix: Actually set the Type property
        }
    }

    public class StartUp : MonoBehaviour
    {
        // Cache startup methods to avoid reflection on every startup
        private static Dictionary<StartUpType, List<MethodInfo>> _cachedStartupMethods;
        private static bool _methodsCached = false;

        public void Awake()
        {
            if (!_methodsCached)
            {
                CacheStartupMethods();
            }

            ExecuteStartupMethods(StartUpType.PreLoad);
        }

        private static void CacheStartupMethods()
        {
            _cachedStartupMethods = new Dictionary<StartUpType, List<MethodInfo>>();
            
            // Initialize lists for each startup type
            foreach (StartUpType startupType in Enum.GetValues(typeof(StartUpType)))
            {
                _cachedStartupMethods[startupType] = new List<MethodInfo>();
            }

            

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                // Skip system assemblies for better performance
                if (IsSystemAssembly(assembly))
                    continue;

                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        // Use GetMethods with binding flags for better performance
                        var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        
                        foreach (var method in methods)
                        {
                            // Quick parameter check
                            if (method.GetParameters().Length > 0)
                                continue;

                            // Check for StartUpMethodAttribute
                            var attribute = method.GetCustomAttribute<StartUpMethodAttribute>(false);
                            if (attribute != null)
                            {
                                _cachedStartupMethods[attribute.Type].Add(method);
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Handle assemblies that can't be loaded
                    Debug.LogWarning($"Could not load types from assembly {assembly.FullName}: {ex.Message}");
                }
            }

            _methodsCached = true;
            
            // Log startup methods found for debugging
            foreach (var kvp in _cachedStartupMethods)
            {
                Debug.Log($"Found {kvp.Value.Count} startup methods for {kvp.Key}");
            }
        }

        private static bool IsSystemAssembly(Assembly assembly)
        {
            var name = assembly.FullName;
            return name.StartsWith("System.") || 
                   name.StartsWith("Microsoft.") || 
                   name.StartsWith("mscorlib") ||
                   name.StartsWith("netstandard") ||
                   name.StartsWith("UnityEngine.") ||
                   name.StartsWith("UnityEditor.");
        }

        public static void ExecuteStartupMethods(StartUpType startupType)
        {
            if (!_methodsCached)
            {
                CacheStartupMethods();
            }

            if (_cachedStartupMethods.TryGetValue(startupType, out var methods))
            {
                foreach (var method in methods)
                {
                    try
                    {
                        method.Invoke(null, null);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error executing startup method {method.DeclaringType?.Name}.{method.Name}: {ex.Message}");
                    }
                }
            }
        }

        // Method to execute BeforeGameplay startup methods
        public void ExecuteBeforeGameplayMethods()
        {
            ExecuteStartupMethods(StartUpType.BeforeGameplay);
        }

        // Method to clear cache if needed (useful for editor scenarios)
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ClearCache()
        {
            _cachedStartupMethods?.Clear();
            _methodsCached = false;
        }
    }
}