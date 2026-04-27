using System;
using _Main.ToolKit.SingletonFeature;
using Main.HitObjectSys.Manager.RunTime.Interface;
using Main.InputSys.Sys.Event;
using MessagePipe;
using UnityEngine;

namespace Main.HitObjectSys.Sys.RayHitObjectSys
{
    [Serializable]
    public class RayHitObjectSystem : Singleton<RayHitObjectSystem>
    {
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
            GlobalMessagePipe.GetSubscriber<Event_InputOnHit>().Subscribe(TryRaycastByHitPosition).AddTo(bag);
            _disposable = bag.Build();
        }

        protected override void Release()
        {
            _disposable.Dispose();
            base.Release();
        }

        #endregion

        #region Raycase Hit Logic

        private void TryRaycastByHitPosition(Event_InputOnHit data)
        {
            Ray ray = Camera.main.ScreenPointToRay(data.HitPosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out IHitAble hitAble))
                {
                    hitAble.OnHitEnter();
                }
            }
        }

        #endregion
    }
}