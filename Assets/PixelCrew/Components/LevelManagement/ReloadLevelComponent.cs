using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrew.Model;
using PixelCrew.Utils;


namespace PixelCrew.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = MainGOsUtils.GetGameSession();
            session.SetData(session.DefaultData);
            
            var scene = SceneManager.GetActiveScene();
            var loader = MainGOsUtils.GetLevelLoader();
            
            loader.LoadLevel(scene.name);
        }
    }
}