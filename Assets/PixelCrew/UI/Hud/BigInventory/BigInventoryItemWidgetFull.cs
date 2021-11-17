using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Hud.BigInventory
{
    public class BigInventoryItemWidgetFull : BigInventoryItemWidgetLite, IDragHandler, IEndDragHandler,IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private BigInventoryController _controller;
        private bool _isDragging = false;
        private RectTransform _thisRect;

        protected override void Start()
        {
            base.Start();
            SetActive(false);
            var parent = GetComponentInParent<Transform>();
            _controller = parent.GetComponentInParent<BigInventoryController>();
            _thisRect = gameObject.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!IsActive) return;
            _isDragging = true;
            _controller.OnStartDragging(_uniqueId, _thisRect.position);
            SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!IsActive && !_isDragging) return;
            _controller.OnDragging();
        }

        public void OnEndDrag(PointerEventData eventData)
        { 
            if(!IsActive && !_isDragging) return;
            _isDragging = false;
            SetActive(true);
            _controller.OnEndDragging();
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