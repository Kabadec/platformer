using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.HeroAll
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
            if (context.canceled && _hero.IsPause == false)
            {
                _hero.Interact();
            }
        }
        public void OnPressS(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.SetSPressed(true);
            }
            if (context.canceled)
            {
                _hero.SetSPressed(false);
            }
        }
        public void OnPressF(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.OnOffFlashLight();
            }
        }
        public void OnPressX(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UseForceShield();
            }
        }

        public void OnPressC(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.TryUseSwordShield();
            }
        
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.canceled && _hero.IsPause == false && _hero.CanControlHero)
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
        public void OnDrop(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.DropFromPlatform();
            }
        }
        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.NextItem();
            }
        }
    }

}