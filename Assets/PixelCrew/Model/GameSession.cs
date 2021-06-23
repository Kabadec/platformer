using UnityEngine;
using System;


namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        [SerializeField] private PlayerData _defaultData;
        public PlayerData DefaultData => _defaultData;

        private void Awake()
        {
            
            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                SetData(_defaultData);
                //Debug.Log("xyi");
            }
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }
        public void SetData(PlayerData data)
        {
            _data.Coins = data.Coins;
            _data.Hp = data.Hp;
            _data.IsArmed = data.IsArmed;
        }
        public void SetDefaultData(PlayerData data)
        {
            _defaultData.Coins = data.Coins;
            _defaultData.Hp = data.Hp;
            _defaultData.IsArmed = data.IsArmed;
        }
    }
}