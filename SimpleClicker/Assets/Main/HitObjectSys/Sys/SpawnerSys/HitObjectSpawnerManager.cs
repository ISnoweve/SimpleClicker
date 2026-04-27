using System;
using _Main.ToolKit.SingletonFeature;
using Main.HitObjectSys.Manager.RunTime;
using MessagePipe;
using UnityEngine;

namespace Main.HitObjectSys.Sys.SpawnerSys
{
    public class HitObjectSpawnerManager : Singleton<HitObjectSpawnerManager>
    {
        [SerializeField] private HitObjectSpawner spawners;
        
        #region Life Cycle

        protected override void Initialize()
        {
            base.Initialize();
            SubscribeEvents();
        }

        private IDisposable _disposable;
        private void SubscribeEvents()
        {
            _disposable?.Dispose();
            var bag = DisposableBag.CreateBuilder();
            _disposable = bag.Build();
        }

        protected override void Release()
        {
            _disposable.Dispose();
            base.Release();
        }

        #endregion

        #region Setting Spawner

        

        #endregion
    }
}