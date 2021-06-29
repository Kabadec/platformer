using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [Range(0, 60)] [SerializeField] private float _delayToDestroy = 0f;
        [SerializeField] private bool _destroyOnStart = false;

        private void Start()
        {
            if (_destroyOnStart)
                DestroyObject();
        }
        public void DestroyObject()
        {
            Destroy(_objectToDestroy, _delayToDestroy);
        }
    }
}