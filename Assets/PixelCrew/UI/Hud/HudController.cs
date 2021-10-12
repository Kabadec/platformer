using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.UI.Widgets.Editor;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;

            OnHealthChanged(_session.Data.Hp.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }
        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }

    }
}