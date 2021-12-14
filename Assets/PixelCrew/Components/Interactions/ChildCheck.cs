using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class ChildCheck : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onHaveNotChild;
        
        private int _diedChild = 0;
        private bool _isActive = true;
        

        private void Update()
        {
            if(!_isActive)
                return;

            if (transform.childCount <= _diedChild)
            {
                _isActive = false;
                _onHaveNotChild?.Invoke();
            }
        }

        public void OnChildDie()
        {
            _diedChild++;
        }
    }
}