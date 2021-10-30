using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] public HealthChangeEvent _onChange;
        [SerializeField] private bool _immune;

        public int Health => _health;
        
        public bool Immune
        {
            get => _immune;
            set => _immune = value;
        }


        public bool ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return false;
            if (healthDelta > 0 && _health >= DefsFacade.I.Player.MaxHealth) return false;
            if (healthDelta < 0 && _immune) return false;

            _health += healthDelta;
            if (_health > DefsFacade.I.Player.MaxHealth)
                _health = DefsFacade.I.Player.MaxHealth;

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
            return true;
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