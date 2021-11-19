using PixelCrew.Model;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Components.Candle
{
    public class CandleComponent : MonoBehaviour
    {
        [SerializeField] private float _decreaseSpeed;
        [SerializeField] [Range(0, 100)] private float _thresholdDecreaseBrightness = 10;
        [SerializeField] private Light2D _sourceLight;
        [SerializeField] private float _maxIntensity = 1;

        private GameSession _session;
        private FloatProperty Oil => _session.Data.Oil;
        private float MaxOil => DefsFacade.I.Player.MaxOil;

        private void Start()
        {
            _session = MainGOsUtils.GetGameSession();
        }

        private void FixedUpdate()
        {
            Oil.Value -= _decreaseSpeed;
            if (Oil.Value <= 0)
                Oil.Value = 0;
            
            var normalizedOilValue = Oil.Value / MaxOil;
            var normalizedThresholdDecreaseBrightness = _thresholdDecreaseBrightness / 100;
            if (normalizedOilValue <= normalizedThresholdDecreaseBrightness)
            {
                var normalizedLightIntensity = normalizedOilValue / normalizedThresholdDecreaseBrightness;
                _sourceLight.intensity = normalizedLightIntensity * _maxIntensity;
            }
            else
                _sourceLight.intensity = _maxIntensity;
        }
    }
}