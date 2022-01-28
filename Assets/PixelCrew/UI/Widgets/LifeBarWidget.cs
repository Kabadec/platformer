using System;
using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class LifeBarWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;
        
        [SerializeField] private bool _saveWorldScale = false;
        [SerializeField] private bool _invertXScale = false;
        [SerializeField] private bool _invertYScale = false;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _maxHp;

        //private Transform _parentTransform;
        private Vector3 _defaultScale;
        //private RectTransform _rectTransform;
        
        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();

            _maxHp = _hp.Health;

            
            //_parentTransform = GetComponentInParent<Transform>();
            //_rectTransform = GetComponent<RectTransform>();
            //Debug.Log(_parentTransform.gameObject.name);

            _defaultScale = transform.localScale;
            //Debug.Log(_defaultScale.x);

                        
            _trash.Retain(_hp._onDie.Subscribe(OnDie));
            _trash.Retain(_hp._onChange.Subscribe(OnHpChanged));
            
        }

        private void Update()
        {
            if(!_saveWorldScale)
                return;

            var parentScale = _hp.gameObject.transform.localScale;
            var parentScaleNormalized = Vector3.one;
            var newScale = Vector3.zero;
            
            if (parentScale.x < 0)
                parentScaleNormalized.x *= -1;
            if (parentScale.y < 0)
                parentScaleNormalized.y *= -1;
            if (parentScale.z < 0)
                parentScaleNormalized.z *= -1;
            
            newScale.x = _defaultScale.x * parentScaleNormalized.x;
            if (_invertXScale)
                newScale.x *= -1;
            newScale.y = _defaultScale.y * parentScaleNormalized.y;
            if (_invertYScale)
                newScale.y *= -1;
            newScale.z = _defaultScale.z * parentScaleNormalized.z;

            transform.localScale = newScale;
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