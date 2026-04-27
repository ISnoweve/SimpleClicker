using System;
using System.Collections.Generic;
using _Main.ToolKit.SingletonFeature;

namespace _Main.ServiceLocatorSys
{
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        private readonly Dictionary<Type, object> _services = new();

        #region Life Cycle

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Release()
        {
            Clear();
            base.Release();
        }

        #endregion 
        
        #region Behaviour

        public static void Register<T>(T service)
        {
            Type type = typeof(T);

            if (!type.IsInterface)return;

            if (Instance._services.ContainsKey(type))return;

            Instance._services.Add(type, service);
        }

        public static void Unregister<T>()
        {
            Type type = typeof(T);

            if (!Instance._services.ContainsKey(type))return;
            
            Instance._services.Remove(type);
        }

        public static T Get<T>()
        {
            Type type = typeof(T);

            if (Instance._services.TryGetValue(type, out object service))
            {
                return (T)service;
            }

            return default;
        }
        
        public static bool IsRegistered<T>()
        {
            Type type = typeof(T);
            return Instance._services.ContainsKey(type);
        }

        private static void Clear()
        {
            Instance._services.Clear();
        }

        #endregion
       
    }
}