using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] private HealthChangeEvent _onChange;


        public void ChangeHealth(int healthDelta)
        {
            if (_health <= 0)
            {
                return;
            }
            _health += healthDelta;
            _onChange?.Invoke(_health);
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            else if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }
            else if (healthDelta > 0)
            {
                _onHealth?.Invoke();
            }
            //Debug.Log($"Ваше здоровье: {_health}");
        }
#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif
        public void SetHealth(int health)
        {
            _health = health;
        }
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}