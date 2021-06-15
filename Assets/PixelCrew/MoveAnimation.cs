using System;
using System.Collections;
using UnityEngine;

namespace Components
{
    public class MoveAnimation : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private MoveState _start;
        [SerializeField] private MoveState _target;
        [SerializeField] private bool _startIsDefault = true;

        private string _currentState;

        private IEnumerator Start()
        {
            transform.position = _startIsDefault ? _start.Position.position : _target.Position.position;
            _currentState = _startIsDefault ? _start.Position.name : _target.Position.name;
            
            yield return new WaitForSeconds(_delay);

            while (gameObject.activeSelf)
            {
                if (_currentState == _start.Position.name)
                {
                    yield return MoveCoroutine(_target);
                }
                if (_currentState == _target.Position.name)
                {
                    yield return MoveCoroutine(_start);
                }
            }
        }

        private IEnumerator MoveCoroutine(MoveState destination)
        {
            _currentState = destination.Position.name;
            Vector3 startPosition = transform.position;
            float moveTime = 0f;
            while (moveTime < destination.TimeToThis)
            {
                moveTime += Time.deltaTime;
                float progress = moveTime / destination.TimeToThis;
                transform.position = Vector3.Lerp(startPosition, destination.Position.position, progress);

                yield return null;
            }
            
            yield return new WaitForSeconds(destination.TimeInThis);
        }
    }

    [Serializable]
    public class MoveState
    {
        [SerializeField] public Transform Position;
        [SerializeField] public float TimeToThis;
        [SerializeField] public float TimeInThis;
    }
}
