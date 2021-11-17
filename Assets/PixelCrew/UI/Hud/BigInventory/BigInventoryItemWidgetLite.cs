using PixelCrew.Model.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.BigInventory
{
    public class BigInventoryItemWidgetLite : InventoryItemWidget
    {
        [SerializeField] protected Image _panel;
        [SerializeField] protected int _uniqueId;

        public int UniqueId
        {
            get => _uniqueId;
            set => _uniqueId = value;
        }

        protected bool IsActive;

        protected override void Start()
        {
            base.Start();
            SetActive(false);
        }
        public void SetActive(bool isActive)
        {
            _panel.gameObject.SetActive(isActive);
            _icon.gameObject.SetActive(isActive);
            _value.gameObject.SetActive(isActive);
            IsActive = isActive;
            if (!isActive)
                _uniqueId = -1;
        }
        public override void SetData(InventoryItemData item, int index)
        {
            base.SetData(item, index);
            _uniqueId = item.UniqueId;
        }
        
    }
}