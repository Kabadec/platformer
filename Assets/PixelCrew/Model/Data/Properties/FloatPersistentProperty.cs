using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class FloatPersistentProperty : PrefsPersistentProperty<float>
    {
        public FloatPersistentProperty(float defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }
        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }
        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key, defaultValue);
        }

    }
}