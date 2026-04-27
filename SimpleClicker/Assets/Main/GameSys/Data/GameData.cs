using System;
using UnityEngine;

namespace Main.GameSys.Data
{
    [Serializable]
    public class GameData
    {
        [SerializeField] private string playerName;
        [SerializeField] private int score;
        [SerializeField] private string dateTime;
    }
}