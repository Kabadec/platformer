using System;
using PixelCrew.Creatures.HeroAll;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Windows.Help
{
    public class HelpWindow : AnimatedWindow
    {
        
        private float _defaultTimeScale;

        private void Awake()
        {
            MainGOsUtils.GetMainHero().IsPause = true;
        }

        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        
        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
            MainGOsUtils.GetMainHero().IsPause = false;
        }
    }
}