using System;
using UnityEngine;
namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public int SwordsAmmo;

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
            // return new PlayerData
            // {
            //     Coins = Coins,
            //     Hp = Hp,
            //     SwordsAmmo = SwordsAmmo
            // };
        }
    }
}