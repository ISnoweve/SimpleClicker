namespace _Main.ToolKit.SingletonFeature
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        protected static T _instance;
        protected static bool _hasInstance = false;
        private static readonly object _lock = new object();
        public static bool HasInstance => _hasInstance;

        public static T Instance {
            get {
                lock (_lock) {
                    if (_instance == null) {
                        _instance = new T();
                        _instance.Initialize();
                        _hasInstance = true;
                    }
                }

                return _instance;
            }
        }

        protected Singleton() { }

        protected virtual void Initialize() { }

        protected virtual void Release() { }

        public static void ClearInstance() {
            lock (_lock) {
                _instance?.Release();
                _instance = null;
                _hasInstance = false;
            }
        }
    }
}