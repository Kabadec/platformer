using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
namespace PixelCrew.Utils
{
    public class CheatController : MonoBehaviour
    {
        private string _currentInput;
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats;

        private float _inputTime;

        void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;

        }
        void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char inputChar)
        {
            _currentInput += inputChar;
            _inputTime = _inputTimeToLive;
            FindAnyCheats();
        }

        private void FindAnyCheats()
        {
            foreach (var cheatItem in _cheats)
            {
                if (_currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }

        void Update()
        {
            if (_inputTime < 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                _inputTime -= Time.deltaTime;
            }
        }
        [Serializable]
        public class CheatItem
        {
            public string Name;
            public UnityEvent Action;
        }
    }
}