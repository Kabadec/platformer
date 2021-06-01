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

        public void ChangeHealth(int changeHealthValue)
        {
            _health += changeHealthValue;
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
            else if (changeHealthValue < 0)
            {
                _onDamage?.Invoke();
            }
            else if (changeHealthValue > 0)
            {
                _onHealth?.Invoke();
            }
            Debug.Log($"Ваше здоровье: {_health}");
        }
        

    }
}