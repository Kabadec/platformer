using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class InfiniteBackground : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container;

        private Bounds _containerBounds;
        private Bounds _allBounds;

        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;
        private Vector3 _screenSize;
        
        private void Start()
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>();
            _containerBounds = sprites[0].bounds;
            foreach (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;

            _boundsToTransformDelta = transform.position - _allBounds.center;
            _containerDelta = _container.position - _allBounds.center;

        }

        private void LateUpdate()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero);
            var max = _camera.ViewportToWorldPoint(Vector3.one);
            _screenSize = new Vector3(max.x - min.x, max.y - min.y);

            _allBounds.center = transform.position - _boundsToTransformDelta;
            var camPosition = _camera.transform.position.x;
            var screenLeft = new Vector3(camPosition - _screenSize.x / 2, _containerBounds.center.y);
            var screenRight = new Vector3(camPosition + _screenSize.x / 2, _containerBounds.center.y);

            if (!_allBounds.Contains(screenLeft))
            {
                InstantiateContainer(_allBounds.min.x - _containerBounds.extents.x);
            }

            if (!_allBounds.Contains(screenRight))
            {
                InstantiateContainer(_allBounds.max.x + _containerBounds.extents.x);
            }
        }

        private void InstantiateContainer(float boundCenterX)
        {
            var newBounds = new Bounds(new Vector3(boundCenterX, _containerBounds.center.y), _containerBounds.size);
            _allBounds.Encapsulate(newBounds);

            _boundsToTransformDelta = transform.position - _allBounds.center;
            var newContainerXPos = boundCenterX + _containerDelta.x;
            var newPosition = new Vector3(newContainerXPos, _container.transform.position.y);

            Instantiate(_container, newPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            GizmosUtils.DrawBounds(_allBounds, Color.magenta);
        }
    }
}