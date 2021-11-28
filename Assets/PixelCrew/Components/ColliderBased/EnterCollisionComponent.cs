using System;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;
namespace PixelCrew.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private bool _activeCollision = true;

        public bool ActiveCollision => _activeCollision;


        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        
        

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(_tag) && _activeCollision)
            {
                _action?.Invoke(other.gameObject);
            }
        }

        public void SetActive(bool isActive)
        {
            _activeCollision = isActive;
        }
    }


}
