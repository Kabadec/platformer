using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.SwordAmmo
{
    public class SwordsAmmoComponent : MonoBehaviour
    {
        [SerializeField] private int _swordsInHolder;
        [SerializeField] private UnityEvent _onTakeSword;
        [SerializeField] private UnityEvent _onSingleShoot;
        [SerializeField] private IntEvent _onMultiShoot;
        [SerializeField] private UnityEvent _onNoAmmo;
        [SerializeField] private IntEvent _onChangeSwordsAmmo;


        public void ModifySwordsAmmo(int swordsInHolderDelta)
        {
            if (swordsInHolderDelta > 0)
            {
                TakeSwordAmmo(swordsInHolderDelta);
            }
            else
            {
                ShootSwords(swordsInHolderDelta);
            }
            //_onChangeSwordsAmmo?.Invoke(_swordsInHolder);
        }

        private void TakeSwordAmmo(int swordsInHolderDelta)
        {
            _swordsInHolder += swordsInHolderDelta;
            _onChangeSwordsAmmo?.Invoke(swordsInHolderDelta);
            _onTakeSword?.Invoke();
        }

        private void ShootSwords(int swordsInHolderDelta)
        {
            var ammoToShoot = -1 * swordsInHolderDelta;
            if (_swordsInHolder > 1)
            {
                var numThrows = Mathf.Min(ammoToShoot, _swordsInHolder - 1);
                if (numThrows == 1)
                    _onSingleShoot?.Invoke();
                else
                    _onMultiShoot?.Invoke(numThrows);

                _swordsInHolder -= numThrows;
                _onChangeSwordsAmmo?.Invoke(-1 * numThrows);
            }
            else
            {
                _onNoAmmo?.Invoke();
            }
        }
        public void SetSwords(int swords)
        {
            _swordsInHolder = swords;
        }
        [Serializable]
        public class IntEvent : UnityEvent<int>
        {

        }
    }
}