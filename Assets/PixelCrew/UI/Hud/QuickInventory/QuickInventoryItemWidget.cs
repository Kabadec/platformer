using PixelCrew.Model.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryItemWidget : InventoryItemWidget, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected GameObject _selection;
        [SerializeField] protected int _uniqueId;

        private QuickInventoryController _controller;
        private int _index;

        public int UniqueId
        {
            get => _uniqueId;
            set => _uniqueId = value;
        }
        public bool IsActive { get; private set; }
        

        
        protected override void Start()
        {
            base.Start();
            var index = Session.QuickInventory.SelectedIndex;
            var parent = GetComponentInParent<Transform>();
            _controller = parent.GetComponentInParent<QuickInventoryController>();
            _selection.gameObject.SetActive(false);
            Trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
        }
        
        public void SetActive(bool isActive)
        {
            _icon.gameObject.SetActive(isActive);
            _value.gameObject.SetActive(isActive);
            IsActive = isActive;
            if (!isActive)
                _uniqueId = -1;
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue);
        }

        public override void SetData(InventoryItemData item, int index)
        {
            _index = index;
            _uniqueId = item.UniqueId;
            base.SetData(item, index);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _controller.MouseOnMe = gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _controller.MouseOnMe = null;
        }
    }
}