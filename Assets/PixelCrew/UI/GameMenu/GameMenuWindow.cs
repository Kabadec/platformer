using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Creatures;

namespace PixelCrew.UI.GameMenu
{
    public class GameMenuWindow : AnimatedWindow
    {
        private Action _closeAction;


        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);

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