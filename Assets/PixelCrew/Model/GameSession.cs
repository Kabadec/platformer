using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposables;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        public PlayerData Data => _data;

        private PlayerData _defaultData;
        public PlayerData DefaultData => _defaultData;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

        private readonly List<string> _checkPoints = new List<string>();

        private void Awake()
        {
            SetDefaultData(_data);
            
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckPoint);
                SetData(_defaultData);
                InitModels();
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkPoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkPoints.Last();
            foreach (var checkPoint in checkPoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }
        public void SetData(PlayerData data)
        {
            _data = data.Clone();
            _trash.Dispose();
            InitModels();
        }
        public void SetDefaultData(PlayerData data)
        {
            _defaultData = data.Clone();
        }
        public bool IsChecked(string id)
        {
            return _checkPoints.Contains(id);
        }
        public void SetChecked(string id)
        {
            if (!_checkPoints.Contains(id))
            {
                SetDefaultData(_data);
                _checkPoints.Add(id);
            }
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }


        
    }
}