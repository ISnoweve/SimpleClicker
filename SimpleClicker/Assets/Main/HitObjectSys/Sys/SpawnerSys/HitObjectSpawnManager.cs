using System;
using _Main.ToolKit.SingletonFeature;
using Main.HitObjectSys.Manager.RunTime;
using MessagePipe;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.HitObjectSys.Sys.SpawnerSys
{
    public class HitObjectSpawnManager : SingletonMonoBehaviour<HitObjectSpawnManager>
    {
        [Title("Spawner Setting")]
        [SerializeField] private GameObject hitObjectPrefab;
        [SerializeField] private GameObject spawnerObject;
        
        [Title("In Game")]
        [SerializeField] private HitObjectSpawner spawners;
        
        #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            SubscribeEvents();
        }

        private IDisposable _disposable;
        private void SubscribeEvents()
        {
            _disposable?.Dispose();
            var bag = DisposableBag.CreateBuilder();
            _disposable = bag.Build();
        }

        protected override void OnDestroy()
        {
            _disposable.Dispose();
            base.OnDestroy();
        }

        #endregion

        #region Setting Spawner

        

        #endregion
    }
}