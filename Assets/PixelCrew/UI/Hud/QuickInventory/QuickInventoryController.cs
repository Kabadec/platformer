using System;
using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;
        [Space] [SerializeField] private int _leftAndRightPadding = 3;
        [SerializeField] private int _widthItem = 15;
        [SerializeField] private int _heightInventory = 24;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private RectTransform _rectTransform;
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>();
            _rectTransform = GetComponent<RectTransform>();
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }

        [ContextMenu("Rebuild")]
        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;
            _dataGroup.SetData(inventory);

            var widthInventory = inventory.Length <= 3
                ? _leftAndRightPadding * 2 + _widthItem * 3
                : _leftAndRightPadding * 2 + _widthItem * inventory.Length;
            
            _rectTransform.sizeDelta = new Vector2(widthInventory, _heightInventory);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}