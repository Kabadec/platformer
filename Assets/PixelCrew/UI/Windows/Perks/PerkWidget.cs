using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isLocked;
        //[SerializeField] private GameObject _isUsed;
        [SerializeField] private GameObject _isSelected;

        private GameSession _sessoin;
        private PerkDef _data;

        private void Start()
        {
            _sessoin = GameSession.Instance;
            UpdateView();
        }

        public void SetData(PerkDef data, int index)
        {
            _data = data;

            if (_sessoin != null)
                UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _isSelected.SetActive(_sessoin.PerksModel.InterfaceSelection.Value ==_data.Id);
            _isLocked.SetActive(!_sessoin.PerksModel.IsUnlockedForWidget(_data.Id));
        }

        public void OnSelect()
        {
            _sessoin.PerksModel.InterfaceSelection.Value = _data.Id;
        }
        
    }
}