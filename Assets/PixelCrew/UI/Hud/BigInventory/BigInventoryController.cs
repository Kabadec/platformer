using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Hud.BigInventory
{
    public class BigInventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject _bigInventoryContainer;
        [SerializeField] private GameObject _itemsContainer;
        [SerializeField] private BigInventoryItemWidgetLite _itemForDrag;
        
        private RectTransform _itemForDragRect;

        public GameObject MouseOnMe { get; set; }
        private int _uniqueIdDraggingItem;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private readonly List<List<BigInventoryItemWidgetFull>> _items = new List<List<BigInventoryItemWidgetFull>>(new List<BigInventoryItemWidgetFull>[4]);

        public GameObject ItemsContainer => _itemsContainer;
        
        private GameSession _session;
        

        private void Start()
        {
            _session = GameSession.Instance;
            _trash.Retain(_session.BigInventory.Subscribe(Rebuild));
            TakeAllItemWidgets();
            DisableAllItems();
            Rebuild();
            _itemForDragRect = _itemForDrag.GetComponent<RectTransform>();
            _bigInventoryContainer.SetActive(false);
        }

        [ContextMenu("Rebuild")]
        private void Rebuild()
        {
            var inventory = _session.BigInventory.Inventory;
            
            DisableAllItems();
            foreach (var itemData in inventory)
            {
                if (itemData.UniqueId == _itemForDrag.UniqueId)
                {
                    _itemForDrag.SetData(itemData, 0);
                    continue;
                }
                var item = _items[itemData.PosBigInventory[0]][itemData.PosBigInventory[1]];
                item.SetData(itemData, 0);
                item.SetActive(true);
            }

        }
        public void OnStartDragging(int uniqueId, Vector3 pos)
        {
            _uniqueIdDraggingItem = uniqueId;
            var draggingItemData = _session.BigInventory.FoundItemByUniqueId(uniqueId);
            _itemForDragRect.position = pos;
            _itemForDrag.SetData(draggingItemData, 0);
            _itemForDrag.SetActive(true);
        }

        public void OnDragging()
        {
            _itemForDragRect.position = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        }

        public void OnEndDragging()
        {
            _itemForDrag.SetActive(false);
            if (MouseOnMe == null)
            {
                if (_session.QuickInventory.MouseOnMe == null)
                {
                    Rebuild();
                    return;
                }

                _session.QuickInventory.PutItemInQuickSlot(_uniqueIdDraggingItem);
                Rebuild();
                return;
            }
            
            var widgetMouseOnMe = MouseOnMe.GetComponent<BigInventoryItemWidgetFull>();
            if (widgetMouseOnMe.UniqueId != -1)
            {
                Rebuild();
                return;
            }
            for (int i = 0; i < _items.Count; i++)
            {
                for (int j = 0; j < _items[i].Count; j++)
                {
                    if (_items[i][j] != widgetMouseOnMe) continue;
                    _session.BigInventory.PutItemInSomeSlot(_uniqueIdDraggingItem, i, j);
                }
            }
            Rebuild();
        }

        private void TakeAllItemWidgets()
        {
            var childCount = _itemsContainer.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var row = _itemsContainer.transform.GetChild(i);
                var childCountRow = row.childCount;
                _items[i] = new List<BigInventoryItemWidgetFull>(4);
                for (int j = 0; j < childCountRow; j++)
                {
                    var item = row.transform.GetChild(j);
                    var widget = item.gameObject.GetComponent<BigInventoryItemWidgetFull>();
                    _items[i].Add(widget);
                }
            }
        }

        private void DisableAllItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                for (int j = 0; j < _items[i].Count; j++)
                {
                    _items[i][j].SetActive(false);
                }
            }
        }

        public void OnOpenInventory()
        {
            _bigInventoryContainer.SetActive(true);
            Rebuild();
        }
        public void OnCloseInventory()
        {
            _bigInventoryContainer.SetActive(false);
        }
        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}