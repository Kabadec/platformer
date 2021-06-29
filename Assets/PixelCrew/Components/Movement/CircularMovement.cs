using System;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;
        private Rigidbody2D[] _childs;
        private float _time;

        private void Awake()
        {
            UpdateComponents();
        }
        private void UpdateComponents()
        {
            _childs = GetComponentsInChildren<Rigidbody2D>();
        }

        private void Update()
        {
            var isAllDead = true;
            for (var i = 0; i < _childs.Length; i++)
            {
                if (_childs[i])
                {
                    //_childs[i].MovePosition(CalcPos(_childs.Length, i));
                    _childs[i].transform.position = CalcPos(_childs.Length, i) + (Vector2)gameObject.transform.position;

                    isAllDead = false;
                }
            }
            if (isAllDead)
                Destroy(gameObject);

            _time += Time.deltaTime;
        }

        private Vector2 CalcPos(int amountSectors, int numSectors)
        {
            var step = 2 * Mathf.PI / amountSectors;
            var angle = step * numSectors;
            var pos = new Vector2(
                Mathf.Cos(angle + _time * _speed) * _radius,
                Mathf.Sin(angle + _time * _speed) * _radius
                );
            return pos;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateComponents();
            for (var i = 0; i < _childs.Length; i++)
            {
                //_childs[i].MovePosition(CalcPos(_childs.Length, i));
                // _childs[i].transform.position = CalcPos(_childs.Length, i);
                _childs[i].transform.position = CalcPos(_childs.Length, i) + (Vector2)gameObject.transform.position;

            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}