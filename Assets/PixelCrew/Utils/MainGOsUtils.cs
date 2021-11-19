﻿using PixelCrew.Creatures.Hero;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;

namespace PixelCrew.Utils
{

    public static class MainGOsUtils
    {
        private const string CameraTag = "MainCamera";
        private const string CanvasTag = "MainUICanvas";
        
        private static Camera _camera;
        private static GameSession _session;
        private static Hero _hero;
        private static Canvas _canvas;
        private static LevelLoader _loader;
        
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
        
    }
}