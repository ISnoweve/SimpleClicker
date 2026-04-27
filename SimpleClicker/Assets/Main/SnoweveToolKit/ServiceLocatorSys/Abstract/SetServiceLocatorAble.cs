namespace _Main.ServiceLocatorSys.Abstract
{
    public abstract class SetServiceLocatorAble<T> where T : class
    {
        protected abstract void InitialSetting(ServiceLocator serviceLocator);
    }
}