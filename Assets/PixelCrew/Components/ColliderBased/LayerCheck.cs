using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layers;
        [SerializeField] protected bool _isTouchingLayer;

        public bool IsTouchingLayer => _isTouchingLayer;
    }
}