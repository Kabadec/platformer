using System;
using PixelCrew.Creatures;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model;
using PixelCrew.UI.Windows;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.InGameMenu
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
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            Close();
        }
        public void OnLangauges()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");

        }

        public void OnExit()
        { 
           SceneManager.LoadScene("MainMenu");
           var session = FindObjectOfType<GameSession>();
           Destroy(session.gameObject);
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }

        private void OnDestroy()
        {
            var hero = FindObjectOfType<Hero>();
            hero.IsPause = false;
            Time.timeScale = _defaultTimeScale;
        }
    }
}