using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrew.Creatures;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {

        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _hero.Interact();
            }
        }
        public void OnPressS(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.SetSPressed(true);
                //Debug.Log(true);
            }
            if (context.canceled)
            {
                _hero.SetSPressed(false);
                //Debug.Log(false);
            }
        }
        public void OnPressH(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UsePotionHealth();
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }
        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.SetShiftPressed(true);
            }
            if (context.canceled)
            {
                _hero.SetShiftPressed(false);

            }
        }

    }

}