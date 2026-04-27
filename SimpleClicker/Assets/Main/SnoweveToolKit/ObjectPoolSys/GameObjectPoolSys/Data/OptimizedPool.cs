using System.Collections.Generic;
using Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Interface;
using UnityEngine;

namespace Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys
{
    public class OptimizedPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _root;
        private readonly Stack<GameObject> _pool = new();
    
        // 效能優化：緩存介面，避免每次都 GetComponent
        private readonly Dictionary<int, IPoolable[]> _interfaceCache = new();
        private readonly int _maxSize;

        public OptimizedPool(GameObject prefab, int initialSize, int maxSize, Transform root)
        {
            _prefab = prefab;
            _maxSize = maxSize;
            _root = root;

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewInstance();
            }
        }

        private GameObject CreateNewInstance()
        {
            GameObject obj = Object.Instantiate(_prefab, _root);
            obj.SetActive(false);
        
            // 預先抓取介面並存入快取
            var poolables = obj.GetComponents<IPoolable>();
            _interfaceCache.Add(obj.GetInstanceID(), poolables);
        
            _pool.Push(obj);
            return obj;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject obj = _pool.Count > 0 ? _pool.Pop() : Object.Instantiate(_prefab, _root);

            // 如果是新產生的（池空了），確保它有快取
            int id = obj.GetInstanceID();
            if (!_interfaceCache.ContainsKey(id))
            {
                _interfaceCache.Add(id, obj.GetComponents<IPoolable>());
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);

            // 執行介面邏輯
            foreach (var p in _interfaceCache[id]) p.OnSpawn();

            return obj;
        }

        public void Release(GameObject obj)
        {
            int id = obj.GetInstanceID();
        
            // 執行清理
            if (_interfaceCache.TryGetValue(id, out var poolables))
            {
                foreach (var p in poolables) p.OnDespawn();
            }

            if (_pool.Count < _maxSize)
            {
                obj.SetActive(false);
                _pool.Push(obj);
            }
            else
            {
                _interfaceCache.Remove(id);
                Object.Destroy(obj);
            }
        }
    }
}