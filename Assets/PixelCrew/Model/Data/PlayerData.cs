using System;
using UnityEngine;
namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        [Space]
        [Space]
        public int Hp;
        public InventoryData Inventory => _inventory;
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