using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ActivePerkWidget : MonoBehaviour
    {
        [SerializeField] private string _idPerk;
        [SerializeField] private Image _icon;
        [SerializeField] private ProgressBarWidget _cooldown;
        [SerializeField] private GameObject _container;

        private float _cooldownPerk = 3;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            var sprite = _session.PerksModel.PerkSprite(_idPerk);
            _icon.sprite = sprite;
            var def = DefsFacade.I.Perks.Get(_idPerk);
            _cooldownPerk = def.Cooldown;

            _trash.Retain(_session.PerksModel.Subscribe(OnActivePerkChanged));
            OnActivePerkChanged();
        }

        private void Update()
        {
            var cooldown = 1 - ((Time.time - _session.PerksModel.GetTimeHowPerkUsed(_idPerk)) / _cooldownPerk);
            if (cooldown < 0)
                cooldown = 0;
            _cooldown.SetProgress(cooldown);
        }

        private void OnActivePerkChanged()
        {
            _container.SetActive(_session.Data.Perks.Unlocked.Contains(_idPerk));
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}