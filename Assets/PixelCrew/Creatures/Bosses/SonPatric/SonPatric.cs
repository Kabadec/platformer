using System.Collections;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Creatures.Bosses.SonPatric.Spikes;
using PixelCrew.Creatures.HeroAll;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Bosses.SonPatric
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SonPatric : MonoBehaviour
    {
        [Header("Run-up")]
        [SerializeField] private float _startRunUpSpeed = 1;
        [SerializeField] private float _accelerationModifier = 1.02f;
        [SerializeField] private float _decelerationModifier = 0.8f;
        [SerializeField] private float _limitSpeed = 50f;
        [SerializeField] private float _stunDuration = 2f;
        [SerializeField] private float _minVelocityForStun = 10f;
        [SerializeField] private UnityEvent _onStunEvent;
        [SerializeField] private UnityEvent _onRestunEvent;

        [Header("Other")]
        [SerializeField] private int _currentStage = 1;
        [SerializeField] private bool _isStunned;
        [SerializeField] private float _jumpVelocity = 10f;
        [SerializeField] private float _scaleSize = 2f;
        [SerializeField] private bool _isFight = false;
        [SerializeField] private SpawnComponent _tentaclesSpawner;
        
        [Header("Death")]
        [SerializeField] private UnityEvent _onDeath;



        private Vector2 _direction = Vector2.right;
        private float _velocityX;
        private Coroutine _decelerationCoroutine;
        private Coroutine _stunCoroutine;
        
        private Hero _hero;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private HealthComponent _health;
        private BossSpikeController _spikeController;
        private CameraShakeEffect _cameraShake;

        private static readonly int IsStunned = Animator.StringToHash("isStunned");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int StartFight = Animator.StringToHash("startFight");
        private static readonly int Health = Animator.StringToHash("health");
        private static readonly int CurrentStage = Animator.StringToHash("currentStage");

        private void Start()
        {
            _hero = MainGOsUtils.GetMainHero();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<HealthComponent>();
            _spikeController = GetComponent<BossSpikeController>();
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _health.Immune.Retain(this);




            //var animation1 = new Animation();
            //animation1.clip.
        }

        [ContextMenu("Lets fight")]
        public void LetsFight()
        {
            _isFight = true;
            _animator.SetTrigger(StartFight);
            UpdateDirectionOurSon();
        }

        private void FixedUpdate()
        {
           if(_decelerationCoroutine != null || _isStunned || !_isFight)
                return;
           
           if (_direction == GetDirectionToHero())
           {
               _velocityX *= _accelerationModifier;

               if (_velocityX >= _limitSpeed)
                   _velocityX = _limitSpeed;
                
               _rigidbody.velocity = new Vector2( _direction.x * _velocityX, _rigidbody.velocity.y);
           }
           else
           {
               _decelerationCoroutine = StartCoroutine(ChangeDirectionCoroutine());
           }
        }

        private IEnumerator ChangeDirectionCoroutine()
        {
            while (true)
            {
                _velocityX *= _decelerationModifier;
                _rigidbody.velocity = new Vector2(_direction.x * _velocityX, _rigidbody.velocity.y);
                if(_velocityX <= 0.5f)
                    break;
                yield return null;
            }
            UpdateDirectionOurSon();
            _decelerationCoroutine = null;
        }


        public void TryStunHim()
        {
            if(Mathf.Abs(_rigidbody.velocity.x) >= _minVelocityForStun)
                StunHim();
        }
        

        [ContextMenu("Stun")]
        private void StunHim()
        {
            if(_stunCoroutine != null)
                StopCoroutine(_stunCoroutine);
            _stunCoroutine = StartCoroutine(StunCoroutine());
        }

        private IEnumerator StunCoroutine()
        {
            SetStun(true);
            _rigidbody.velocity = Vector2.zero;
            _cameraShake.Shake(0.5f, 5);
            
            yield return new WaitForSeconds(_stunDuration);
            
            SetStun(false);
            UpdateDirectionOurSon();
            
            switch (_currentStage)
            { 
                case 1:
                    break;
                case 2:
                    _isStunned = true;
                    Jump();
                    break;
                case 3:
                    _isStunned = true;
                    Jump();
                    break;
                }
            
            _stunCoroutine = null;
        }

        public void OnJumpEndSecondStage()
        {
            _spikeController.LetsSpike();
            _isStunned = false;
            _cameraShake.Shake(1, 10);
        }

        public void OnJumpEndThirdStage()
        {
            _tentaclesSpawner.Spawn();
            _spikeController.LetsSpike();
            _isStunned = false;
            _cameraShake.Shake(2, 20);
        }

        private void OnDeath()
        {
            _isFight = false;
            if(_stunCoroutine != null)
                StopCoroutine(_stunCoroutine);
            _stunCoroutine = null;
            _onDeath?.Invoke();
        }
        

        private void UpdateDirectionOurSon()
        {
            _direction = GetDirectionToHero();
            UpdateSpriteDirection(_direction);
            _velocityX = _startRunUpSpeed;
        }

        private void SetStun(bool isStunned)
        {
            _isStunned = isStunned;
            _animator.SetBool(IsStunned, isStunned);

            if (isStunned)
            {
                _health.Immune.Release(this);
                _onStunEvent?.Invoke();
            }
            else
            {
                _health.Immune.Retain(this);
                _onRestunEvent?.Invoke();
            }
        }


        private Vector2 GetDirectionToHero()
        {
            var direction = _hero.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private void UpdateSpriteDirection(Vector2 direction)
        {
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1* _scaleSize, _scaleSize, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(_scaleSize, _scaleSize, 1);
            }
        }

        public void OnHealthChanged()
        {
            _animator.SetInteger(Health, _health.Health);

            if (_health.Health >= 66)
                _currentStage = 1;
            else if (_health.Health >= 33)
                _currentStage = 2;
            else if (_health.Health < 33 && _health.Health >= 1)
                _currentStage = 3;
            else if (_health.Health <= 1)
            {
                OnDeath();
                _currentStage = 0;
            }
            _animator.SetInteger(CurrentStage, _currentStage);
        }

        private void Jump()
        {
            _rigidbody.AddForce(new Vector2(0, _jumpVelocity), ForceMode2D.Impulse);
        }
    }
}