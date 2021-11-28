using System;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private Action _closeAction;
        private float _defaultTimeScale;

        
        protected override void Start()
        {
            base.Start();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnResumeGame()
        {
            Close();
        }

        public void OnRestartGame()
        {
            var loader = MainGOsUtils.GetLevelLoader();
            var scene = SceneManager.GetActiveScene();
            loader.LoadLevel(scene.name);
            Close();
        }
        public void OnLangauges()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnExit()
        { 
            Time.timeScale = _defaultTimeScale;
            
            var loader = MainGOsUtils.GetLevelLoader();
            loader.LoadLevel("MainMenu");
            
           var session = GameSession.Instance;
           Destroy(session.gameObject);
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }

        private void OnDestroy()
        {
            var hero = MainGOsUtils.GetMainHero();
            if(hero != null)
                hero.IsPause = false;
            Time.timeScale = _defaultTimeScale;
        }
    }
}