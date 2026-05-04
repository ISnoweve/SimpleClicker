using UnityEngine;

namespace Main.HitObjectSys.Data.HitObjectMoveData
{
    //[CreateAssetMenu(fileName = "HitObjectMoveBase", menuName = "MenuName/HitObject/Base", order = 0)]
    public abstract class HitObjectMoveBaseData : ScriptableObject
    {
        public abstract void Move(Transform hitObject, float deltaTime);
    }
}