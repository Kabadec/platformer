using System;
using UnityEngine;
namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        protected TPropertyType _stored;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }
        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _value;
                Write(value);
                _stored = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        public void Validate()
        {
            if (!_stored.Equals(_value))
                Value = _value;
        }

        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}