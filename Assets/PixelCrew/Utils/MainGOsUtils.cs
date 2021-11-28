using PixelCrew.Creatures.HeroAll;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace PixelCrew.Utils
{

    public static class MainGOsUtils
    {
        private const string CameraTag = "MainCamera";
        private const string CanvasTag = "MainUICanvas";
        private const string GlobalVolumeTag = "GlobalVolume";
        
        
        private static Camera _camera;
        private static Hero _hero;
        private static Canvas _canvas;
        private static LevelLoader _loader;
        private static Volume _globalVolume;
        
        public static Camera GetMainCamera()
        {
            if (_camera != null) return _camera;
            
            var cameras = Object.FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {
                if(!camera.CompareTag(CameraTag)) continue;
                _camera = camera;
                return _camera;
            }
            return default;
        }
        
        public static Hero GetMainHero()
        {
            if (_hero != null) return _hero;

            return _hero = Object.FindObjectOfType<Hero>();
        }
        
        public static LevelLoader GetLevelLoader()
        {
            if (_loader != null) return _loader;

            return _loader = Object.FindObjectOfType<LevelLoader>();
        }

        public static Canvas GetMainCanvas()
        {
            if (_canvas != null) return _canvas;
            
            return _canvas = GameObject.FindWithTag(CanvasTag).GetComponent<Canvas>();
        }
        
        public static Volume GetGlobalVolume()
        {
            if (_globalVolume != null) return _globalVolume;
            
            return _globalVolume = GameObject.FindWithTag(GlobalVolumeTag).GetComponent<Volume>();
            
            //var volumes = FindObjectsOfType<Volume>();
            //foreach(var volume in volumes)
            //{
            //  if(!volume.isGlobal) continue;
            //  volume is globalVolume
            //}
        }
        
    }
}