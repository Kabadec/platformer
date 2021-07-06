using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class ColliderCheck : LayerCheck
    {
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _isTouchingLayer = false;
            if (_collider.IsTouchingLayers(_layers))
            {
                _isTouchingLayer = true;
            }

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            _isTouchingLayer = false;
            if (_collider.IsTouchingLayers(_layers))
            {
                _isTouchingLayer = true;
            }
        }
    }
}