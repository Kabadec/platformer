using System.Collections;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;
using PixelCrew.Utils;
using PixelCrew.Components.GoBased;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemBrain : MonoBehaviour
    {
        [Header("Shoot Preferences")]
        [SerializeField] private float _cooldownShoot = 0.5f;
        [SerializeField] private float _cooldownBurst = 3f;
        [SerializeField] private float _cooldownChecking = 0.1f;

        [Header("Vision Preferences")]
        [SerializeField] private float _widthVision = 5f;
        [SerializeField] private float _modHeightVision = 0.4f;

        [Header("Head Preferences")]
        [Range(1, 10)] [SerializeField] private int _numHeads = 3;
        [SerializeField] private float _heightHead = 0.6875f;
        [SerializeField] private TotemHeads[] _heads;

        [Space] [SerializeField] private bool _invertXScale = false;
        [SerializeField] private UnityEvent _onDie;
        
        private Animator[] _animators;
        private bool _canShooting = false;

        private DestroyObjectComponent _destroyObjectComponent;
        private BoxCollider2D _colliderVision;

        private static readonly int ShootKey = Animator.StringToHash("attack");

        private void Start()
        {
            _destroyObjectComponent = GetComponent<DestroyObjectComponent>();
            _colliderVision = gameObject.GetComponent<BoxCollider2D>();
            RespawnTotem();
            GettingAnimators();
            StartCoroutine(Checking());
        }
        private IEnumerator Checking()
        {
            ResizeVisionCollider(gameObject.transform.childCount);
            if (_canShooting)
                StartCoroutine(Shooting());
            else
            {
                yield return new WaitForSeconds(_cooldownChecking);
                StartCoroutine(Checking());
            }
        }

        private IEnumerator Shooting()
        {
            var isAllDead = true;
            for (var i = 0; i < _animators.Length; i++)
            {
                if (_animators[i])
                {
                    isAllDead = false;
                    _animators[i].SetTrigger(ShootKey);
                }
                else
                {
                    continue;
                }
                yield return new WaitForSeconds(_cooldownShoot);
            }
            if (isAllDead)
            {
                _onDie?.Invoke();
                _destroyObjectComponent.DestroyObject();
            }
            yield return new WaitForSeconds(_cooldownBurst);

            StartCoroutine(Checking());
        }

        [ContextMenu("RespawnTotem")]
        private void RespawnTotem()
        {
            DestroyChildrens();
            SpawnTotem();
            ResizeVisionCollider(_numHeads);
        }

        private void SpawnTotem()
        {
            for (var i = 0; i < _numHeads; i++)
            {
                var lookHead = (int)Random.Range(0f, _heads.Length);
                if (i == _numHeads - 1)
                {
                    SpawnSoloHead(i, lookHead, true);
                }
                else
                {
                    SpawnSoloHead(i, lookHead, false);
                }
            }
        }
        private void SpawnSoloHead(int numberHead, int lookHead, bool isMain)
        {
            GameObject instantiate;
            if (isMain)
                instantiate = Instantiate(_heads[lookHead].HeadMain, new Vector3(0, 0, 0), Quaternion.identity);
            else
                instantiate = Instantiate(_heads[lookHead].Head, new Vector3(0, 0, 0), Quaternion.identity);
            
            instantiate.transform.parent = gameObject.transform;
            var parentPosition = instantiate.transform.parent.position;
            
            instantiate.transform.position = new Vector3(parentPosition.x, parentPosition.y + _heightHead * (numberHead + 0.5f), parentPosition.z);

            if (_invertXScale)
            {
                var instScale = instantiate.transform.localScale;
                instScale.x *= -1;
                instantiate.transform.localScale = instScale;
            }
                
            
        }
        private void ResizeVisionCollider(int numHeads)
        {
            if (!_colliderVision)
                _colliderVision = gameObject.GetComponent<BoxCollider2D>();
            _colliderVision.size = new Vector2(_widthVision, numHeads * _heightHead + _modHeightVision);
            _colliderVision.offset = new Vector2(-1 * _widthVision / 2, (numHeads * _heightHead + _modHeightVision) / 2);
        }

        private void DestroyChildrens()
        {
            var tempArray = new GameObject[gameObject.transform.childCount];

            for (int i = 0; i < tempArray.Length; i++)
                tempArray[i] = gameObject.transform.GetChild(i).gameObject;

            foreach (var child in tempArray)
            {
                if (!Application.isPlaying)
                    DestroyImmediate(child);
                else
                    Destroy(child);
            }
        }

        private void GettingAnimators()
        {
            var childCount = gameObject.transform.childCount;
            _animators = new Animator[childCount];
            for (var i = 0; i < childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                _animators[i] = child.gameObject.GetComponent<Animator>();
            }
        }

        public void SetCanShooting(bool canShooting)
        {
            _canShooting = canShooting;
        }
        [Serializable]
        public class TotemHeads
        {
            [SerializeField] private GameObject _headMain;
            [SerializeField] private GameObject _head;


            public GameObject HeadMain => _headMain;
            public GameObject Head => _head;
        }

    }
}