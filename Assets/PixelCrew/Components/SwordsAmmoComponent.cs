using System;
using UnityEngine;
using UnityEngine.Events;


public class SwordsAmmoComponent : MonoBehaviour
{
    [SerializeField] private int _swordsInHolder;
    [SerializeField] private UnityEvent _onTakeSword;
    [SerializeField] private UnityEvent _goArmSword;
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
        _onChangeSwordsAmmo?.Invoke(_swordsInHolder);
    }

    private void TakeSwordAmmo(int swordsInHolderDelta)
    {
        _swordsInHolder += swordsInHolderDelta;
        _onChangeSwordsAmmo?.Invoke(_swordsInHolder);
        _onTakeSword?.Invoke();
        if (_swordsInHolder <= 0)
        {
            _goArmSword?.Invoke();
        }
    }

    private void ShootSwords(int swordsInHolderDelta)
    {
        var ammoToShoot = -1 * swordsInHolderDelta;
        if (_swordsInHolder > 1)
        {
            if (ammoToShoot == 1)
            {
                _onSingleShoot?.Invoke();
                _swordsInHolder--;
            }
            else
            {
                if (_swordsInHolder > ammoToShoot)
                {
                    _onMultiShoot?.Invoke(ammoToShoot);
                    _swordsInHolder -= ammoToShoot;
                }
                else
                {
                    _onMultiShoot?.Invoke(_swordsInHolder - 1);
                    _swordsInHolder = 1;
                }
            }
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
