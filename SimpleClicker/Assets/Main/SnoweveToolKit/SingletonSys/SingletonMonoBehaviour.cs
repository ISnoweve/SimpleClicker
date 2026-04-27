using UnityEditor;
using UnityEngine;

namespace _Main.ToolKit.SingletonFeature
{
    [DisallowMultipleComponent]
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T _instance;
        protected static bool _hasInstance = false;
        private static readonly object _lock = new object();
        protected virtual bool IsDontDestroyOnLoad => false;
        public static bool HasInstance => _hasInstance;
        
        public static T Instance {
            get {
                if (_instance != null) return _instance;

#if UNITY_EDITOR
                if (EditorApplication.isPlaying &&
                    !EditorApplication.isPlayingOrWillChangePlaymode)
                    return null;
#endif


                lock (_lock) {
                    if (_instance == null) {
                        GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
                        _instance = singletonObject.AddComponent<T>();

                        if (_instance is SingletonMonoBehaviour<T> { IsDontDestroyOnLoad: true }) {
                            DontDestroyOnLoad(singletonObject);
                        }

                        _hasInstance = true;
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake() {
            if(_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            
            if (IsDontDestroyOnLoad) {
                DontDestroyOnLoad(gameObject);
            }
            
            // if (_instance == null) {
            //     _instance = this as T;
            //     if (IsDontDestroyOnLoad) {
            //         DontDestroyOnLoad(gameObject);
            //     }
            //
            //     _hasInstance = true;
            // }
            // else if (_instance != this) {
            //     Destroy(gameObject);
            // }
        }
        
        protected virtual void OnDestroy() {
            if (_instance == this) {
                _instance = null;
                _hasInstance = false;
            }
        }
    }
}