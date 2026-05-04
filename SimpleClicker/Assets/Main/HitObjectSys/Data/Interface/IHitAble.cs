namespace Main.HitObjectSys.Manager.RunTime.Interface
{
    public interface IHitAble
    {
        public void OnHitEnter();
        public void OnHitStay();
        public void OnHitExit();
    }
}