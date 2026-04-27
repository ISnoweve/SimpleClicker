using System;
using _Main.ToolKit.SingletonFeature;
using Main.InputSys.Sys.Event;
using MessagePipe;
using UnityEngine;

namespace Main.InputSys.View.Sound_InputHit
{
    public class InputHitSoundView : SingletonMonoBehaviour<InputHitSoundView>
    {
        [SerializeField] private AudioClip hitSoundClip;
        protected override bool IsDontDestroyOnLoad => true;

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
            GlobalMessagePipe.GetSubscriber<Event_InputOnHit>().Subscribe(PlayHitSound).AddTo(bag);
            _disposable = bag.Build();
        }

        protected override void OnDestroy()
        {
            _disposable?.Dispose();
            base.OnDestroy();
        }

        #endregion

        #region Play Sound

        private void PlayHitSound(Event_InputOnHit data)
        {
            AudioManager.Instance.PlaySFX2D(hitSoundClip);
        }

        #endregion
    }
}