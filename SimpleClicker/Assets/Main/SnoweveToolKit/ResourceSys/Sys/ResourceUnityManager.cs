using System;
using System.Collections.Generic;
using _Main.ToolKit.SingletonFeature;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Main.ResourceSys.Sys
{
    [Serializable]
    public sealed class ResourceUnityManager : Singleton<ResourceUnityManager>
    {
        private static readonly Dictionary<string, Object> _cache = new();

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemRegistrationReset()
        {
            _cache.Clear();
            ClearInstance();
        }
#endif

        public static T Load<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out var cached)) return cached as T;

            var asset = Resources.Load<T>(path);
            if (asset != null)
                _cache[path] = asset;
            else
                Debug.LogWarning($"[ResourceManager] 無法載入資源: {path}");

            return asset;
        }

        public static void Unload(string path)
        {
            if (_cache.TryGetValue(path, out var asset))
            {
                Resources.UnloadAsset(asset);
                _cache.Remove(path);
            }
        }

        public static void UnloadAll()
        {
            foreach (var kvp in _cache) Resources.UnloadAsset(kvp.Value);

            _cache.Clear();
        }

        public static void ForceUnloadUnused()
        {
            _cache.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}