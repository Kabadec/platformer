using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew
{
    public class PalmPlatform : MonoBehaviour
    {
        [SerializeField] private float _modifierYUp;
        [SerializeField] private float _modifierYDown;
        private Collider2D _collider;
        private Hero _hero;

        private bool _isHeroEnter = false;


        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
            _collider = GetComponent<Collider2D>();
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
                gameObject.layer = 8;//8: Ground
            }
            else
            {
                _collider.isTrigger = true;
                gameObject.layer = 0;//0: Default
            }
        }

        public void SetIsHeroEnter(bool isHeroEnter)
        {
            _isHeroEnter = isHeroEnter;
        }


    }
}