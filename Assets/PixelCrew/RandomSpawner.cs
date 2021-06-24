using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using UnityEngine.Events;

namespace PixelCrew.Utils
{
    public class RandomSpawner : MonoBehaviour
    {
        [Header("Spawn bound:")]
        [SerializeField]
        private float _sectorAngle = 60;

        [SerializeField] private float _sectorRotation;

        [Header("Array spawn objects:")]
        [Space]
        [SerializeField] private ParticleObject[] _particles;

        [Header("Spawn params:")]
        [Space]
        [SerializeField] private float _itemPerBurst = 2;

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;
        [SerializeField] private float _numParticles = 200;

        private void Start()
        {
            //Restart();
        }
        private void OnEnable()
        {
            Restart();
        }

        private Coroutine _routine;

        [ContextMenu("Restart")]
        public void Restart()
        {
            TryStopRoutine();

            _routine = StartCoroutine(StartSpawn());
        }

        private IEnumerator StartSpawn()
        {
            var sumChances = 0f;
            for (var i = 0; i < _particles.Length; i++)
            {
                sumChances += _particles[i].ChanceDrop;
            }

            for (var i = 0; i < _numParticles; i++)
            {
                for (var j = 0; j < _itemPerBurst; j++)
                {
                    var resultDrop = Random.Range(0f, sumChances);
                    var sum0 = 0f;
                    var sum1 = 0f;
                    for (var k = 0; k < _particles.Length; k++)
                    {
                        sum0 += _particles[k].ChanceDrop;
                        if (sum1 <= resultDrop && resultDrop < sum0)
                        {
                            Spawn(_particles[k].Particle);
                        }
                        sum1 += _particles[k].ChanceDrop;
                    }
                }
                yield return new WaitForSeconds(_waitTime);
            }
        }

        [ContextMenu("Spawn one")]
        private void Spawn(GameObject particle)
        {
            var instance = Instantiate(particle, transform.position, Quaternion.identity);
            var delta = 0.001f;
            instance.transform.position += new Vector3(Random.Range(-delta, delta), Random.Range(-delta, delta), 0f);

            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);
            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            var rightBound = GetUnitOnCircle(middleAngleDelta);
            Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, 1);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, 1);
        }

        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }

        private void OnDisable()
        {
            TryStopRoutine();
        }

        private void OnDestroy()
        {
            TryStopRoutine();
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        [Serializable]
        public class ParticleObject
        {
            [SerializeField] private GameObject _particle;
            [SerializeField] private float _chanceDrop;

            public GameObject Particle => _particle;
            public float ChanceDrop => _chanceDrop;
        }
    }
}