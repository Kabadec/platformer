using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class HealthAnimationGlue : MonoBehaviour
    {
        [SerializeField] private HealthComponent _hp;
        [SerializeField] private Animator _animator;
        private static readonly int Health = Animator.StringToHash("health");

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _trash.Retain(_hp._onChange.Subscribe(OnHealthChanged));
            OnHealthChanged(_hp.Health);
        }

        private void OnHealthChanged(int health)
        {
            _animator.SetInteger(Health, health);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}