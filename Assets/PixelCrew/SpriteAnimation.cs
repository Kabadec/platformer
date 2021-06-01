using System;
using UnityEngine;
using UnityEngine.Events;
//using SpriteAnimation.Clips;


namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {

        [SerializeField] private int _frameRate;

        [SerializeField] private Clips[] _clips;
        [SerializeField] private UnityEvent _onComplete;


        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;
        private int _currentClipIndex;



        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSpriteIndex = 0;
            _currentClipIndex = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;
            var currentClip = _clips[_currentClipIndex];




            if (_currentSpriteIndex >= currentClip._sprites.Length)
            {
                if (currentClip._loop)
                {
                    _currentSpriteIndex = 0;
                    return;
                }
                else if (currentClip._allowNextClip)
                {
                    _currentClipIndex++;
                    if (_currentClipIndex >= _clips.Length)
                    {
                        _currentClipIndex = 0;
                        
                    }
                        _currentSpriteIndex = 0;

                }
                else
                {
                    enabled = false;
                    _onComplete?.Invoke();
                    return;
                }
            }

            _renderer.sprite = _clips[_currentClipIndex]._sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetClip(string name)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == name)
                {
                    _currentClipIndex = i;
                    _currentSpriteIndex = 0;
                    _nextFrameTime = Time.time + _secondsPerFrame;
                    return;
                }
            }
        }
        [Serializable]
        public class Clips
        {
            [SerializeField] public string Name;
            [SerializeField] public Sprite[] _sprites;
            [SerializeField] public bool _loop;
            [SerializeField] public bool _allowNextClip;
        }
    }
}

