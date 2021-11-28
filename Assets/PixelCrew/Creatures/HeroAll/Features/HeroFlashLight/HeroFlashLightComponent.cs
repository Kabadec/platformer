using PixelCrew.Model;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.HeroAll.Features.HeroFlashLight
{
    public class HeroFlashLightComponent : MonoBehaviour
    {
        [SerializeField] private float _decreaseOilSpeedPerSecond;
        [SerializeField] [Range(0, 100)] private float _thresholdDecreaseBrightness = 10;
        [SerializeField] private Light2D _sourceLight;
        
        private float _maxIntensity;

        private GameSession _session;
        private FloatProperty Oil => _session.Data.Oil;
        private float MaxOil => DefsFacade.I.Player.MaxOil;

        private void Start()
        {
            _session = GameSession.Instance;
            _maxIntensity = _sourceLight.intensity;
        }

        private void Update()
        {
            Oil.Value -= _decreaseOilSpeedPerSecond * Time.deltaTime;
            if (Oil.Value <= 0)
                Oil.Value = 0;

            var newLightIntensity = Mathf.Clamp(Oil.Value / _thresholdDecreaseBrightness, 0,  1);
            _sourceLight.intensity = newLightIntensity;
            
            if(Oil.Value == 0f)
                gameObject.SetActive(false);

            // var normalizedOilValue = Oil.Value / MaxOil;
            // var normalizedThresholdDecreaseBrightness = _thresholdDecreaseBrightness / 100;
            // if (normalizedOilValue <= normalizedThresholdDecreaseBrightness)
            // {
            //     var normalizedLightIntensity = normalizedOilValue / normalizedThresholdDecreaseBrightness;
            //     _sourceLight.intensity = normalizedLightIntensity * _maxIntensity;
            // }
            // else
            //     _sourceLight.intensity = _maxIntensity;
        }
    }
}