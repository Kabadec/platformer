using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using System;


namespace PixelCrew.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingsComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;
        private AudioSource _source;
        private FloatPersistentProperty _model;


        private void Awake()
        {

        }
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            //GameSettings.I.Music
            _model = FindProperty();
            _model.OnChanged += OnSoundSettingsChanged;
            OnSoundSettingsChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingsChanged(float newValue, float oldValue)
        {
            if (_source == null)
                _source = GetComponent<AudioSource>();

            _source.volume = newValue;
        }
        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSetting.Music:
                    return GameSettings.I.Music;
                case SoundSetting.Sfx:
                    return GameSettings.I.Sfx;
            }
            throw new ArgumentException("Undefined mode");
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingsChanged;
        }
    }
}