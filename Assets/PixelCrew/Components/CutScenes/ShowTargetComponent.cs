using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.CutScenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private CameraStateController _controller;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private UnityEvent _onDelay;
        
        private Coroutine _coroutine;

        private void OnValidate()
        {
            if (_controller == null)
                _controller = FindObjectOfType<CameraStateController>();

        }
        public void ShowTarget()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(MoveBack());
        }

        private IEnumerator MoveBack()
        {
            yield return new WaitForSeconds(_delay);

            _onDelay?.Invoke();
            _controller.SetState(false);
        }
    }
}