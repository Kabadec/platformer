
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{


    public class HeroInputReader : MonoBehaviour
    {

        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }



        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                //_hero.SaySomething();
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
            //тут нужно выяснить какая кнопка E или S нажата
        }
        public void GoDown(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                _hero.SetSPressed(true);
            }
            if (context.canceled)
            {
                _hero.SetSPressed(false);
            }
        }


    }

}