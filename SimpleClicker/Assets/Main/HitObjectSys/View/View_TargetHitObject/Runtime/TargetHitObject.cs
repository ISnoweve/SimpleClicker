using System;
using Main.HitObjectSys.Manager.RunTime.Interface;
using UnityEngine;

namespace Main.HitObjectSys.Manager.RunTime
{
    [Serializable]
    public class TargetHitObject : MonoBehaviour,IHitAble
    {
        #region HitEnter

        public void OnHitEnter()
        {
        }

        #endregion

        #region HitStay

        public void OnHitStay()
        {
        }

        #endregion

        #region HitExit

        public void OnHitExit()
        {
        }

        #endregion
    }
}