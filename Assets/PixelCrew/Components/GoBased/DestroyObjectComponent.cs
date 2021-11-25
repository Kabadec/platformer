using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private string _id;
        [Range(0, 60)] [SerializeField] private float _delayToDestroy = 0f;
        [SerializeField] private bool _destroyOnStart = false;
        //[SerializeField] private bool _storeState;
        [SerializeField] private RestoreStateComponent _state;
        
        private void Start()
        {
            if (_destroyOnStart)
                DestroyObject();
        }
        public void DestroyObject()
        {
            if (_state != null)
                GameSession.Instance.StoreState(_state.Id);
            Destroy(_objectToDestroy, _delayToDestroy);
        }
    }
}