using EventSys.Interface;
using Main.HitObjectSys.Manager.RunTime.Interface;
using UnityEngine;

namespace Main.InputSys.Sys.Event
{
    public readonly struct Event_InputOnHit : IEventData
    {
        private readonly Vector2 _hitPosition;

        public Vector2 HitPosition => _hitPosition;
        
        public Event_InputOnHit(Vector2 hitPosition)
        {
            _hitPosition = hitPosition;
        }
    }
}