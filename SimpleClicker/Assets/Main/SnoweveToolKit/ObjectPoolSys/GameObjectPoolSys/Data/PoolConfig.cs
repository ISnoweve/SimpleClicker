using System;
using UnityEngine;

namespace Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Data
{
    [Serializable]
    public struct PoolConfig
    {
        public GameObject prefab;
        public int initialSize;
        public int maxSize;
    }
}