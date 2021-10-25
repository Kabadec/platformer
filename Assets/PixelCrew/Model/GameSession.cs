﻿using UnityEngine;
using System;
using PixelCrew.Model.Data;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        private PlayerData _defaultData;
        public PlayerData DefaultData => _defaultData;
        public QuickInventoryModel QuickInventory { get; private set; }

        private void Awake()
        {
            LoadHud();
            SetDefaultData(_data);
            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                
                SetData(_defaultData);
                InitModels();
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(Data);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
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
            _data = data.Clone();
        }
        public void SetDefaultData(PlayerData data)
        {
            _defaultData = data.Clone();
        }
    }
}