using UnityEngine;

namespace PixelCrew.Effects
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _effectValue;
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _deltaX = 0;

        
        private float _startX;
        private Vector3 _screenSize;

        private void Start()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero);
            var max = _camera.ViewportToWorldPoint(Vector3.one);
            _screenSize = new Vector3(max.x - min.x, max.y - min.y);
            _startX = transform.position.x;
        }

        private void LateUpdate()
        {
            var currentPosition = transform.position;
            var deltaX = _followTarget.position.x * _effectValue;
            transform.position = new Vector3(_startX + _deltaX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}