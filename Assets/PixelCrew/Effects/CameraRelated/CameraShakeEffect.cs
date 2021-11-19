using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShakeEffect : MonoBehaviour
    {
        [SerializeField] private float _animationTime = 0.3f;
        [SerializeField] private float _intensity = 3f;

        private CinemachineBasicMultiChannelPerlin _cameraNoise;

        private Coroutine _coroutine;

        private void Awake()
        {
            var virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            if (_coroutine != null)
                StopAnimation();
            _coroutine = StartCoroutine(StartAnimation());
        }

        

        private IEnumerator StartAnimation()
        {
            _cameraNoise.m_FrequencyGain = _intensity;
            yield return new WaitForSeconds(_animationTime);
            StopAnimation();
        }
        private void StopAnimation()
        {
            _cameraNoise.m_FrequencyGain = 0f;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

    }
}