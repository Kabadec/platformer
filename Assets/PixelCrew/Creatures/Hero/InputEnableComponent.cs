using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;

        private void Start()
        {
            var hero = MainGOsUtils.GetMainHero();
            _input = hero.GetComponent<PlayerInput>();
        }

        public void SetInput(bool isEnabled)
        {
            _input.enabled = isEnabled;
        }
    }
}