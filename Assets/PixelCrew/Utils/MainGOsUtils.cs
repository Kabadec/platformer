using PixelCrew.Creatures.Hero;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Utils
{

    public static class MainGOsUtils
    {
        private const string CameraTag = "MainCamera";
        private const string CanvasTag = "MainCanvas";
        
        private static Camera _camera;
        private static GameSession _session;
        private static Hero _hero;
        private static Canvas _canvas;
        
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

        public static GameSession GetGameSession()
        {
            if (_session != null) return _session;

            return _session = Object.FindObjectOfType<GameSession>();
        }

        public static Hero GetMainHero()
        {
            if (_hero != null) return _hero;

            return _hero = Object.FindObjectOfType<Hero>();
        }

        public static Canvas GetMainCanvas()
        {
            if (_canvas != null) return _canvas;
            
            var canvases = Object.FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (!canvas.CompareTag(CanvasTag)) continue;
                _canvas = canvas;
                return _canvas;
            }

            return default;
        }
        
    }
}