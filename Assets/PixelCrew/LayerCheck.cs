using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask[] _layers;
        [SerializeField] private bool _isTouchingLayer;

        public bool IsTouchingLayer => _isTouchingLayer;
        private Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _isTouchingLayer = false;
            for (var i = 0; i < _layers.Length; i++)
            {
                if (_collider.IsTouchingLayers(_layers[i]))
                {
                    _isTouchingLayer = true;
                }
            }

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            _isTouchingLayer = false;
            for (var i = 0; i < _layers.Length; i++)
            {
                if (_collider.IsTouchingLayers(_layers[i]))
                {
                    _isTouchingLayer = true;
                }
            }
        }
    }
}