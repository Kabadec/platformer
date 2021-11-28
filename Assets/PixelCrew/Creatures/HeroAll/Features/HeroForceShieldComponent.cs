using System.Collections;
using PixelCrew.Components.Health;
using UnityEngine;

namespace PixelCrew.Creatures.HeroAll.Features
{
    public class HeroForceShieldComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private float _durationForceShield = 3;

        private Coroutine _coroutine;

        public void Use()
        {
            _health.Immune.Retain(this);
            TryStop();
            gameObject.SetActive(true);
            _coroutine = StartCoroutine(ForceShieldCoroutine());
        }

        private void TryStop()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }
        private IEnumerator ForceShieldCoroutine()
        {
            yield return new WaitForSeconds(_durationForceShield);
            _health.Immune.Release(this);
            gameObject.SetActive(false);
        }
    }
}