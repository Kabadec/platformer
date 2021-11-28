using UnityEngine;
using PixelCrew.Creatures.HeroAll;

namespace PixelCrew.Components
{
    public class PalmPlatformComponent : MonoBehaviour
    {
        [SerializeField] private float _modifierYUp;
        [SerializeField] private float _modifierYDown;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _defaultLayer;
        private Collider2D _collider;
        private Hero _hero;

        private bool _isHeroEnter = false;


        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        private void FixedUpdate()
        {
            if (_hero.GetGoDownWithPlatform())
            {
                SetActive(false);
                _isHeroEnter = false;
                return;
            }
            UpdateActive();
        }
        private void UpdateActive()
        {
            if (_hero.transform.position.y > gameObject.transform.position.y + _modifierYUp)
            {
                SetActive(true);
            }
            else if (_hero.transform.position.y < gameObject.transform.position.y + _modifierYDown)
            {
                SetActive(false);
            }
            else
            {
                if (_isHeroEnter)
                {
                    SetActive(false);
                }
                else
                {
                    SetActive(true);
                }
            }
        }
        private void SetActive(bool active)
        {
            if (active)
            {
                _collider.isTrigger = false;

                gameObject.layer = (int)Mathf.Log(_groundLayer.value, 2);//8: Ground
            }
            else
            {
                _collider.isTrigger = true;
                gameObject.layer = (int)Mathf.Log(_defaultLayer.value, 2);//0: Default
            }
        }

        public void SetIsHeroEnter(bool isHeroEnter)
        {
            _isHeroEnter = isHeroEnter;
        }


    }
}