namespace Main.SnoweveToolKit.ObjectPoolSys.GameObjectPoolSys.Interface
{
    public interface IPoolable
    {
        /// <summary>
        /// 當物件從池中被取出時觸發
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// 當物件被回收回池中時觸發
        /// </summary>
        void OnDespawn();
    }
}