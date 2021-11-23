using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace PixelCrew.Components.PostEffects
{
    public class SetPostEffectProfile : MonoBehaviour
    {
        [SerializeField] private VolumeProfile _profile;

        public void Set()
        {
            var globalVolume = MainGOsUtils.GetGlobalVolume();
            globalVolume.profile = _profile;
        }
    }
}