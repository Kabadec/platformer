using System;
using System.Linq;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    public class LocalizeImage : AbstractLocalizeComponent
    {
        [SerializeField] private IconId[] _icons;
        [SerializeField] private Image _icon;
        protected override void Localize()
        {
            var iconData = _icons.FirstOrDefault(x => x.Id == LocalizationManager.I.LocaleKey);
            if(iconData != null)
                _icon.sprite = iconData.Icon;
        }
        
        [Serializable]
        public class IconId
        {
            public string Id;
            public Sprite Icon;
        }
    }
}