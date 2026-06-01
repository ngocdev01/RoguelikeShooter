using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NgocDev.Core.ServiceLocator
{
    public class ServiceLocatorInstance : MonoBehaviour
    {
        [SerializeField] private bool _logInitialization = true;

        private readonly HashSet<Type> _serviceTypes = new HashSet<Type>();

        private void Awake()
        {
            InitializeServices();
        }

        private void InitializeServices()
        {
            if (_logInitialization)
                Debug.Log("[ServiceLocator] Initializing services...");

            CollectServiceTypes();
            var graph = BuildDependencyGraph();
            var sortedServices = TopologicalSort(graph);

            ServiceLocator.ClearServices();

            foreach (var serviceType in sortedServices)
            {
                var serviceObject = new GameObject(serviceType.Name);
                serviceObject.transform.SetParent(transform, false);

                var serviceInstance = serviceObject.AddComponent(serviceType);
                ServiceLocator.RegisterService(serviceType, serviceInstance);

                if (_logInitialization)
                    Debug.Log($"[ServiceLocator] Registered: {serviceType.Name}");
            }

            if (_logInitialization)
                Debug.Log($"[ServiceLocator] Initialized {sortedServices.Count} services.");
        }

        private void CollectServiceTypes()
        {
            _serviceTypes.Clear();

            // Only scan assemblies that might contain our services
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && 
                            !a.FullName.StartsWith("System") && 
                            !a.FullName.StartsWith("mscorlib"));

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(MonoService).IsAssignableFrom(type) && 
                            !type.IsInterface && 
                            !type.IsAbstract)
                        {
                            _serviceTypes.Add(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[ServiceLocator] Failed to scan assembly {assembly.FullName}: {ex.Message}");
                }
            }
        }

        private Dictionary<Type, List<Type>> BuildDependencyGraph()
        {
            var graph = new Dictionary<Type, List<Type>>();

            // Initialize all service nodes
            foreach (var serviceType in _serviceTypes)
            {
                graph[serviceType] = new List<Type>();
            }

            // Build dependencies
            foreach (var serviceType in _serviceTypes)
            {
                // Handle InitializeAfter: this service depends on others
                var afterAttrs = (InitializeAfterAttribute[])serviceType
                    .GetCustomAttributes(typeof(InitializeAfterAttribute), false);

                foreach (var attr in afterAttrs)
                {
                    if (_serviceTypes.Contains(attr.ServiceType))
                    {
                        graph[serviceType].Add(attr.ServiceType);
                    }
                    else
                    {
                        Debug.LogWarning($"[ServiceLocator] {serviceType.Name} depends on {attr.ServiceType.Name}, but it's not a registered service.");
                    }
                }

                // Handle InitializeeBefore: other services depend on this
                var beforeAttrs = (InitializeBeforeAttribute[])serviceType
                    .GetCustomAttributes(typeof(InitializeBeforeAttribute), false);

                foreach (var attr in beforeAttrs)
                {
                    if (_serviceTypes.Contains(attr.ServiceType))
                    {
                        graph[attr.ServiceType].Add(serviceType);
                    }
                }
            }

            return graph;
        }

        private List<Type> TopologicalSort(Dictionary<Type, List<Type>> graph)
        {
            var visited = new HashSet<Type>();
            var inStack = new HashSet<Type>(); // For cycle detection
            var sorted = new List<Type>();

            foreach (var serviceType in graph.Keys)
            {
                if (!visited.Contains(serviceType))
                {
                    if (!DFS(serviceType, graph, visited, inStack, sorted))
                    {
                        Debug.LogError("[ServiceLocator] Circular dependency detected! Services may not initialize correctly.");
                        break;
                    }
                }
            }

            return sorted;
        }

        private bool DFS(Type node, Dictionary<Type, List<Type>> graph, 
            HashSet<Type> visited, HashSet<Type> inStack, List<Type> sorted)
        {
            visited.Add(node);
            inStack.Add(node);

            if (graph.TryGetValue(node, out var dependencies))
            {
                foreach (var dependency in dependencies)
                {
                    if (inStack.Contains(dependency))
                    {
                        Debug.LogError($"[ServiceLocator] Circular dependency: {node.Name} <-> {dependency.Name}");
                        return false;
                    }

                    if (!visited.Contains(dependency) && graph.ContainsKey(dependency))
                    {
                        if (!DFS(dependency, graph, visited, inStack, sorted))
                            return false;
                    }
                }
            }

            inStack.Remove(node);
            sorted.Add(node);
            return true;
        }
    }
}