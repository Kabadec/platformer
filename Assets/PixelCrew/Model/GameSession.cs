using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposables;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        
        public static GameSession Instance { get; private set; }
        
        public PlayerData Data => _data;
        private PlayerData _defaultData;
        public PlayerData DefaultData => _defaultData;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        public QuickInventoryModel QuickInventory { get; private set; }
        public BigInventoryModel BigInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }
        

        private readonly List<string> _checkPoints = new List<string>();

        private void Awake()
        {
            // level_start
            // level_complete
            // level_index
            SetDefaultData(_data);
            _checkPoints.Add(_defaultCheckPoint);
            
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint, _levelIndex);
                existsSession.BigInventory.UpdateInventory();
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(_defaultCheckPoint, _levelIndex);
                SetData(_defaultData);
                InitModels();
                BigInventory.UpdateInventory();
            }
        }
        
        

        private void StartSession(string defaultCheckPoint, int levelIndex)
        {
            //SetChecked(defaultCheckPoint);
            //TrackSessionStart(levelIndex);
            //InitModels();
            LoadUIs();
            SpawnHero();
        }

        private void TrackSessionStart(int levelIndex)
        {
            var eventParams = new Dictionary<string, object>
            {
                {"level_index", levelIndex}
            };
            
            AnalyticsEvent.Custom("level_start", eventParams);
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

            BigInventory = new BigInventoryModel(_data);
            _trash.Retain(BigInventory);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            LoadOnScreenControls();
        }

        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadOnScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);

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
            SetDefaultData(_data);
            _defaultCheckPoint = id;
            _checkPoints.Add(id);
            //Debug.LogError("default checkpoint changed", this);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
        }

        private List<string> _removedItems = new List<string>();

        public bool RestoreState(string itemID)
        {
            return _removedItems.Contains(itemID);
        }

        public void StoreState(string itemID)
        {
            if(!_removedItems.Contains(itemID))
                _removedItems.Add(itemID);
        }
    }
}