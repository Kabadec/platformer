using System;
using PixelCrew.Components.Health;
using PixelCrew.UI.Widgets.Editor;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _maxHp;
        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();

            _maxHp = _hp.Health;

            _trash.Retain(_hp._onDie.Subscribe(OnDie));
            _trash.Retain(_hp._onChange.Subscribe(OnHpChanged));
            
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnHpChanged(int hp)
        {
            var progress = (float) hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}