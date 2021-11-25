using System;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets.Editor;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ActivePerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarWidget _cooldown;

        private float _cooldownPerk = 3;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;

            _trash.Retain(_session.PerksModel.Subscribe(OnActivePerkChanged));
            OnActivePerkChanged();
        }

        private void Update()
        {
            var cooldown = 1 - ((Time.time - _session.PerksModel.TimeHowPerkUsed) / _cooldownPerk);
            if (cooldown < 0)
                cooldown = 0;
            _cooldown.SetProgress(cooldown);
        }

        private void OnActivePerkChanged()
        {
            var sprite = _session.PerksModel.ActivePerkSprite();
            if(sprite == null)
                _icon.gameObject.SetActive(false);
            else
            {
                _icon.gameObject.SetActive(true);
                _icon.sprite = sprite;
            }

            var def = DefsFacade.I.Perks.Get(_session.PerksModel.Used);
            _cooldownPerk = def.Cooldown;
            _session.PerksModel.SetTimeHowPerkUsed(-10f);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}