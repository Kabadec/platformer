using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animatorKey;
        private void Start()
        {
            _animator.SetBool(_animatorKey, _state);
        }
        
        [ContextMenu("Switch")]
        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animatorKey, _state);
        }
    }
}