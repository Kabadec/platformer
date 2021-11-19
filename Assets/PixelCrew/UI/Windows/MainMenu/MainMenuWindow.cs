using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;
        

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                MainGOsUtils.GetLevelLoader().LoadLevel("Level1");
            };
            Close();
        }
        
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnLangauges()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");

        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();

        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }

    }
}