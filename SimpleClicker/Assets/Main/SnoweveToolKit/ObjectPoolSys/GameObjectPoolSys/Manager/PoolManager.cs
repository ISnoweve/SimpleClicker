using System.Collections.Generic;
using _Main.ToolKit.SingletonFeature;
using Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Data;
using UnityEngine;

namespace Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Manager
{
    public class PoolManager : SingletonMonoBehaviour<PoolManager>
    {
        [SerializeField] private List<PoolConfig> poolConfigs;

        // 運行中的物件 ID -> 所屬物件池 (用於自動回收)
        private readonly Dictionary<int, OptimizedPool> _instanceIdToPool = new();

        // Prefab ID -> 物件池
        private readonly Dictionary<int, OptimizedPool> _prefabIdToPool = new();

        protected override void Awake()
        {
            base.Awake();
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var config in poolConfigs)
            {
                if (config.prefab == null) continue;

                var prefabId = config.prefab.GetInstanceID();

                // 建立父節點優化 Hierarchy 視窗
                var folder = new GameObject($"Pool_{config.prefab.name}");
                folder.transform.SetParent(transform);

                var pool = new OptimizedPool(config.prefab, config.initialSize, config.maxSize, folder.transform);
                _prefabIdToPool.Add(prefabId, pool);
            }
        }

        /// <summary>
        ///     從池中取出物件
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int prefabId = prefab.GetInstanceID();

            // 如果沒預設池，現場建立一個預設規模的
            if (!_prefabIdToPool.TryGetValue(prefabId, out OptimizedPool pool))
            {
                GameObject folder = new GameObject($"Pool_{prefab.name} (Auto)");
                folder.transform.SetParent(transform);
                pool = new OptimizedPool(prefab, 0, 100, folder.transform);
                _prefabIdToPool.Add(prefabId, pool);
            }

            GameObject obj = pool.Get(position, rotation);

            // 紀錄此物件 ID 對應的池，方便回收
            int instanceId = obj.GetInstanceID();
            if (!_instanceIdToPool.ContainsKey(instanceId)) _instanceIdToPool.Add(instanceId, pool);

            return obj;
        }

        /// <summary>
        ///     將物件回收至池中
        /// </summary>
        public void Despawn(GameObject obj)
        {
            if (obj == null) return;

            int instanceId = obj.GetInstanceID();

            if (_instanceIdToPool.TryGetValue(instanceId, out OptimizedPool pool))
            {
                pool.Release(obj);
            }
            else
            {
                // 如果此物件不屬於任何池，則直接銷毀
                DebugHelper.LogWarningEditor($"Object {obj.name} was not managed by PoolManager. Destroying...");
                Destroy(obj);
            }
        }
    }
}