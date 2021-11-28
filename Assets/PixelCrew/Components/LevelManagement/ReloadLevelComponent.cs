using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine.InputSystem;


namespace PixelCrew.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var hero = MainGOsUtils.GetMainHero();
            var heroInput = hero.GetComponent<PlayerInput>();
            heroInput.DeactivateInput();
            
            var session = GameSession.Instance;
            session.SetData(session.DefaultData);
            
            var scene = SceneManager.GetActiveScene();
            var loader = MainGOsUtils.GetLevelLoader();
            
            loader.LoadLevel(scene.name);
        }
    }
}