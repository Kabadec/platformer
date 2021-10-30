using System;
using PixelCrew.Creatures;
using PixelCrew.Model;
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
            //игра возобновляется
            //Time.timeScale = 1;
            //_creatures = 
            //_creatures.SetActive(false);
            var parentCreatures = GameObject.FindWithTag("CREATURES");
            var creatures = parentCreatures.GetComponentsInChildren<Creature>(true);
            foreach (var creature in creatures)
            {
                creature.gameObject.SetActive(true);
            }

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
           /* _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();*/
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
            Time.timeScale = _defaultTimeScale;
        }
    }
}