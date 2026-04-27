using System;
using MessagePipe;
using UnityEngine;

namespace EventSys.Runtime
{
    [DefaultExecutionOrder(int.MinValue)]
    public static partial class MessagePipeInitializer
    {
        private static IServiceProvider _provider;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init() {
            if (_provider != null) return; // 避免重複（關閉 Domain Reload 時特別重要）
            BuiltinContainerBuilder builder = new BuiltinContainerBuilder();
            builder.AddMessagePipe(o => {
#if UNITY_EDITOR
                o.HandlingSubscribeDisposedPolicy = HandlingSubscribeDisposedPolicy.Throw; // 開發期較好
                o.EnableCaptureStackTrace = true;
#else
                o.HandlingSubscribeDisposedPolicy = HandlingSubscribeDisposedPolicy.Ignore;
                o.EnableCaptureStackTrace = false;
#endif
                o.InstanceLifetime = InstanceLifetime.Singleton;
            });
            RegisterEventBrokers(builder);
            _provider = builder.BuildServiceProvider();
            GlobalMessagePipe.SetProvider(_provider);
            Application.quitting += OnQuit;
        }

        static partial void RegisterEventBrokers(BuiltinContainerBuilder builder);

        private static void OnQuit() {
            (_provider as IDisposable)?.Dispose();
            _provider = null;
            Application.quitting -= OnQuit;
        }
    }
}