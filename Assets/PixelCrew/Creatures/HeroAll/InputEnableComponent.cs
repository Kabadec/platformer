using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.HeroAll
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;

        private void Start()
        {
            var hero = MainGOsUtils.GetMainHero();
            if(hero != null)
                _input = hero.GetComponent<PlayerInput>();
            else
                Debug.Log("hero is null!");
        }

        public void SetInput(bool isEnabled)
        {
            _input.enabled = isEnabled;
        }
    }
}