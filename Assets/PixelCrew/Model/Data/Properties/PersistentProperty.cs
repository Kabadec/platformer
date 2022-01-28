using System;
using UnityEngine;
using PixelCrew.Utils.Disposables;
namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
    {
        private TPropertyType _stored;
        private readonly TPropertyType _defaultValue;
        
        protected PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }
        public override TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _value;
                Write(value);
                _stored = _value = value;
                InvokeChangedEvent(_value, oldValue);
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