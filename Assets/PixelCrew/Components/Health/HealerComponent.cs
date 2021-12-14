using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    [RequireComponent(typeof(ModifyHealthComponent))]
    public class HealerComponent : MonoBehaviour
    {
        [SerializeField] private int _heal = 1;
        [SerializeField] private float _timeBetweenHeals = 1f;

        private ModifyHealthComponent _modifyHealthComponent;

        private GameObject _target;

        private Coroutine _healCoroutine;

        private void Start()
        {
            _modifyHealthComponent = GetComponent<ModifyHealthComponent>();
        }

        public void StartHeal(GameObject target)
        {
            _target = target;
            if(_healCoroutine != null)
                StopCoroutine(_healCoroutine);
            _healCoroutine = StartCoroutine(HealCoroutine());
        }

        private IEnumerator HealCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timeBetweenHeals);
                _modifyHealthComponent.SetHpDelta(_heal);
                _modifyHealthComponent.ModifyHealth(_target);
            }
        }

        public void StopHeal()
        {
            if(_healCoroutine != null)
                StopCoroutine(_healCoroutine);
            _healCoroutine = null;
        }
    }
}