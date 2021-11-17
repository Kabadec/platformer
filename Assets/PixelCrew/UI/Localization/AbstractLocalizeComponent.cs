using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    
    public abstract class AbstractLocalizeComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            LocalizationManager.I.OnLocaleChanged += OnLocalChanged;
            Localize();
        }

        protected virtual void OnLocalChanged()
        {
            Localize();
        }

        protected abstract void Localize();

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= OnLocalChanged;
        }
    }
}