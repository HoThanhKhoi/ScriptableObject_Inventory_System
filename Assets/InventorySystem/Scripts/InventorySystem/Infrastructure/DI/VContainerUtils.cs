using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public static class VContainerUtils
{
    private static IObjectResolver _globalResolver;
    private static readonly Dictionary<Scene, IObjectResolver> _sceneResolvers = new();
    private static readonly Dictionary<Type, object> _manualRegistry = new();
    private static readonly object _lock = new(); // Thread safety

    /// <summary>
    /// Initializes the VContainer global resolver.
    /// </summary>
    public static void Initialize(IObjectResolver resolver)
    {
        _globalResolver = resolver;
    }

    /// <summary>
    /// Registers a resolver for a specific scene.
    /// </summary>
    public static void RegisterSceneResolver(Scene scene, IObjectResolver resolver)
    {
        lock (_lock)
        {
            if (!_sceneResolvers.ContainsKey(scene))
            {
                _sceneResolvers[scene] = resolver;
            }
        }
    }

    /// <summary>
    /// Resolves a service from the scene-specific container or falls back to global.
    /// </summary>
    public static T ResolveForScene<T>(Scene scene) where T : class
    {
        lock (_lock)
        {
            return _sceneResolvers.TryGetValue(scene, out var resolver) ? resolver.Resolve<T>() : Resolve<T>();
        }
    }

    /// <summary>
    /// Resolves a registered service or falls back to a manually registered instance.
    /// </summary>
    public static T Resolve<T>() where T : class
    {
        lock (_lock)
        {
            if (_globalResolver?.TryResolve<T>(out var service) == true)
                return service;

            return _manualRegistry.TryGetValue(typeof(T), out var instance) ? instance as T : null;
        }
    }

    /// <summary>
    /// Resolves a service lazily, delaying the resolution until it is first accessed.
    /// </summary>
    public static Lazy<T> LazyResolve<T>() where T : class
    {
        return new Lazy<T>(() => Resolve<T>());
    }

    /// <summary>
    /// Registers an instance in the manual registry.
    /// </summary>
    public static void RegisterInstance<T>(T instance) where T : class
    {
        lock (_lock)
        {
            if (_manualRegistry.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"[VContainerUtils] Instance of {typeof(T)} is already registered.");
                return;
            }
            _manualRegistry[typeof(T)] = instance;
        }
    }

    /// <summary>
    /// Unregisters an instance from the manual registry.
    /// </summary>
    public static void Unregister<T>() where T : class
    {
        lock (_lock)
        {
            if (_manualRegistry.Remove(typeof(T)))
            {
                Debug.Log($"[VContainerUtils] Unregistered instance of {typeof(T)}.");
            }
            else
            {
                Debug.LogWarning($"[VContainerUtils] Attempted to unregister {typeof(T)}, but it was not found.");
            }
        }
    }

    /// <summary>
    /// Injects dependencies into an existing component.
    /// </summary>
    public static void InjectComponent<T>(T component) where T : Component
    {
        if (_globalResolver == null)
        {
            Debug.LogWarning($"[VContainerUtils] Cannot inject into {typeof(T)}: Resolver is null!");
            return;
        }

        _globalResolver.Inject(component);
    }

    /// <summary>
    /// Injects dependencies into a GameObject and its components.
    /// </summary>
    public static void InjectGameObject(GameObject gameObject)
    {
        if (_globalResolver == null)
        {
            Debug.LogWarning($"[VContainerUtils] Cannot inject into GameObject {gameObject.name}: Resolver is null!");
            return;
        }

        foreach (var component in gameObject.GetComponentsInChildren<Component>())
        {
            _globalResolver.Inject(component);
        }
    }

    /// <summary>
    /// Logs all manually registered services for debugging.
    /// </summary>
    public static void LogAllManualRegistrations()
    {
        lock (_lock)
        {
            if (_manualRegistry.Count == 0)
            {
                Debug.LogWarning("[VContainerUtils] No manually registered services.");
                return;
            }

            Debug.Log("[VContainerUtils] Manually Registered Services:");
            foreach (var entry in _manualRegistry)
            {
                Debug.Log($"- {entry.Key.Name} : {entry.Value}");
            }
        }
    }
    /// <summary>
    /// Automatically injects the dependencies of a component and its root GameObject.
    /// </summary>
    public static void AutoInjectSelf<T>(T component) where T : Component
    {
        if (_globalResolver == null)
        {
            Debug.LogWarning($"[VContainerUtils] Cannot auto-inject {typeof(T)}: Resolver is null!");
            return;
        }
        // Find the root GameObject
        GameObject root = component.transform.root.gameObject;
        // Inject the root GameObject and all its components
        InjectGameObject(root);
        Debug.Log($"[VContainerUtils] Auto-injected {typeof(T).Name} on {component.gameObject.name} (Root: {root.name})");
    }

    /// <summary>
    /// Cleans up scene-based resolvers when a scene is unloaded.
    /// </summary>
    public static void OnSceneUnloaded(Scene scene)
    {
        lock (_lock)
        {
            if (_sceneResolvers.Remove(scene))
            {
                Debug.Log($"[VContainerUtils] Removed scene resolver for {scene.name}");
            }
        }
    }
}
