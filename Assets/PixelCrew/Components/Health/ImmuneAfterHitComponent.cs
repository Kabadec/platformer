using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class ImmuneAfterHitComponent : MonoBehaviour
    {
        [SerializeField] private float _immuneTime;
        private HealthComponent _health;
        private Coroutine _coroutine;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _health._onDamage.AddListener(OnDamage);
        }
        private void OnDestroy()
        {
            _health._onDamage.RemoveListener(OnDamage);
        }


        private void OnDamage()
        {
            TryStop();
            _coroutine = StartCoroutine(MakeImmune());
        }
        private void TryStop()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator MakeImmune()
        {
            _health.Immune = true;
            yield return new WaitForSeconds(_immuneTime);
            _health.Immune = false;
            _coroutine = null;
        }
    }
}