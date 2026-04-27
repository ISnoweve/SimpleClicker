using EventSys.Interface;

namespace Main.HitObjectSys.Manager.RunTime
{
    public readonly struct TargetOnHitEnter : IEventData
    {
        private readonly float _msTimeSpawnToExpire;
        private readonly TargetHitObject _targetHitObject;
        
        public float MsTimeSpawnToExpire => _msTimeSpawnToExpire;
        public TargetHitObject TargetHitObject => _targetHitObject;

        public TargetOnHitEnter(float msTimeSpawnToExpire, TargetHitObject targetHitObject)
        {
            _msTimeSpawnToExpire = msTimeSpawnToExpire;
            _targetHitObject = targetHitObject;
        }
    }
}