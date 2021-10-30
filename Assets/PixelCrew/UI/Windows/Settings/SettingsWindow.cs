using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.UI.Widgets;
using PixelCrew.Model.Data;
namespace PixelCrew.UI.SettingsWindow
{


    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

        protected override void Start()
        {
            base.Start();

            //GameSettings.I.Music;
            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);

        }
    }
}