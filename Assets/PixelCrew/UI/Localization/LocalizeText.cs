using System;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : AbstractLocalizeComponent
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _capitalize;

        private Text _text;

        protected override void Awake()
        {
            _text = GetComponent<Text>();
            base.Awake();
        }

        protected override void Localize()
        {
            var localized = LocalizationManager.I.Localize(_key);
            _text.text = _capitalize ? localized.ToUpper() : localized;
        }
    }
}