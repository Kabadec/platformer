using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Bosses.Patric.Bombs
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private float _ttl;
        [SerializeField] private UnityEvent _onDetonate;
        
        private Coroutine _coroutine;

        private void OnEnable()
        {
            TryStop();
            _coroutine = StartCoroutine(WaitAndDetonate());
        }

        private IEnumerator WaitAndDetonate()
        {
            yield return new WaitForSeconds(_ttl);
            Detonate();
        }

        public void Detonate()
        {
            TryStop();
            _onDetonate?.Invoke();
        }

        private void OnDisable()
        {
            TryStop();
        }

        private void TryStop()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}