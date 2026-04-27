using _Main.ToolKit.SingletonFeature;
using Main.HitObjectGameSys;
using Main.HitObjectSys.Sys.RayHitObjectSys;
using Main.InputSys.Sys;
using UnityEngine;

namespace Main.GameSys.Sys
{
    public class InitGameSystem : SingletonMonoBehaviour<InitGameSystem>
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private RayHitObjectSystem rayHitObjectSystem;
        [SerializeField] private HitObjectGameSystem hitObjectGameSystem;
        
        protected override void Awake()
        {
            base.Awake();
            SetUpGameSystem();
        }

        private void SetUpGameSystem()
        {
            inputSystem = InputSystem.Instance;
            rayHitObjectSystem = RayHitObjectSystem.Instance;
            hitObjectGameSystem = HitObjectGameSystem.Instance;
        }
        
        
        protected override void OnDestroy()
        {
            ClearInstanceGameSystem();
            base.OnDestroy();
        }
        
        private void ClearInstanceGameSystem()
        {
            InputSystem.ClearInstance();
            RayHitObjectSystem.ClearInstance();
            HitObjectGameSystem.ClearInstance();
        }
    }
}