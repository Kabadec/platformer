
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{


    public class HeroInputReader : MonoBehaviour
    {

        [SerializeField] private Hero _hero;
        private Vector2 _direction = Vector2.zero;
        public void OnMovement(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
            _hero.SetDirection(_direction);
        }

        

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.SaySomething();
            }
        }
    }
}