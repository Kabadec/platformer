using System;
using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChangeEmissionColor : MonoBehaviour
    {
        [ColorUsage(true, true)] [SerializeField]
        private Color _color;

        private SpriteRenderer _sprite;
        private static readonly int EmissionColor = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        [ContextMenu("Change color")]
        public void Change()
        {
            _sprite.material.SetColor(EmissionColor, _color);
        }
    }
}