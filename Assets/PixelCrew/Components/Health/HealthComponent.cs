using System;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHealth;
        [SerializeField] public HealthChangeEvent _onChange;

        private Lock _immune = new Lock();

        private int _maxHealth;
        
        public int Health => _health;
        
        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            SetHealth(_health);
        }

        public Lock Immune => _immune;

        public bool ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return false;
            if (healthDelta > 0 && _health >= _maxHealth) return false;
            if (healthDelta < 0 && _immune.IsLocked) return false;

            _health += healthDelta;
            if (_health > _maxHealth)
                _health = _maxHealth;

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
            _health = _maxHealth = health;
        }
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}