using System;
using _Main.ToolKit.SingletonFeature;
using Main.HitObjectGameSys.Event;
using Main.HitObjectSys.Manager.RunTime;
using Main.InputSys.Sys.Event;
using MessagePipe;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.HitObjectGameSys
{
    [Serializable]
    public class HitObjectGameSystem : Singleton<HitObjectGameSystem>
    {
        [Title("Game Setting")]
        [SerializeField] private int timeLimit = 60;
        
        [Title("Score"),ReadOnly]
        [SerializeField] private int score;
        
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
            GlobalMessagePipe.GetSubscriber<TargetOnHitEnter>().Subscribe(TryUpdateScore).AddTo(bag);
            _disposable = bag.Build();
        }

        protected override void Release()
        {
            _disposable.Dispose();
            base.Release();
        }

        #endregion

        #region Start Game

        private void StartGame()
        {
            ResetScore();
        }

        #endregion

        #region End Game
        

        #endregion
        
        #region Score Logic

        private void ResetScore()
        {
            score = 0;
        }
        
        private void TryUpdateScore(TargetOnHitEnter data)
        {
            score++;
            ScoreUpdate eventData = new ScoreUpdate(score);
            GlobalMessagePipe.GetPublisher<ScoreUpdate>().Publish(eventData);
        }

        #endregion
    }
}