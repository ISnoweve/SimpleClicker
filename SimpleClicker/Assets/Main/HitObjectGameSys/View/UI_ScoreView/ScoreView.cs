using System;
using _Main.ToolKit.SingletonFeature;
using Main.HitObjectGameSys.Event;
using MessagePipe;
using TMPro;
using UnityEngine;

namespace Main.HitObjectGameSys.View.UI_ScoreView
{
    public class ScoreView : SingletonMonoBehaviour<ScoreView>
    {
        [SerializeField] private TMP_Text scoreText;


        #region Life Cycle

        protected override void Awake()
        {
            base.Awake();
            SubscribeEvent();
        }
        
        private IDisposable _disposable;
        private void SubscribeEvent()
        {
            _disposable?.Dispose();
            var bag = DisposableBag.CreateBuilder();
            GlobalMessagePipe.GetSubscriber<ScoreUpdate>().Subscribe(UpdateScoreView).AddTo(bag);
            _disposable = bag.Build();
        }

        protected override void OnDestroy()
        {
            _disposable?.Dispose();
            base.OnDestroy();
        }

        #endregion

        private void UpdateScoreView(ScoreUpdate data)
        {
            scoreText.text = $"Score: {data.Score}";
        }
    }
}