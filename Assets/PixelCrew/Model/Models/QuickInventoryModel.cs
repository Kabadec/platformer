using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.Hud.QuickInventory;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public readonly QuickInventoryItemWidget[] Items = new QuickInventoryItemWidget[3];
        
        private QuickInventoryItemWidget[] _items => GameSession.Instance.QuickInventory.Items;

        public readonly IntProperty SelectedIndex = new IntProperty();

        public event Action OnChanged;

        public GameObject MouseOnMe { get; set; }

        public InventoryItemData SelectedItem =>  _data.Inventory.GetItemByUniqueId(Items[SelectedIndex.Value].UniqueId);

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;
            _data.Inventory.OnChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].UniqueId == -1)
                {
                    _items[i].SetActive(false);
                    return;
                }
                var itemData = _data.Inventory.GetItemByUniqueId(_items[i].UniqueId);
                if (itemData == null)
                {
                    _items[i].SetActive(false);
                    return;
                }
                _items[i].SetActive(true);
                _items[i].SetData(itemData, i);
            }
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, _items.Length - 1);
            
            OnChanged?.Invoke();
        }
        
        public void PutItemInQuickSlot(int uniqueId)
        {
            var itemData = _data.Inventory.GetItemByUniqueId(uniqueId);
            var itemDef = DefsFacade.I.Itemses.Get(itemData.Id);
            var isUsable = itemDef.HasTag(ItemTag.Usable);

            if (!isUsable) return;
            
            var widgetMouseOnMe = MouseOnMe.GetComponent<QuickInventoryItemWidget>();
            for (int i = 0; i < _items.Length; i++)
            {
                if (widgetMouseOnMe == _items[i])
                {
                    _items[i].SetData(itemData, i);
                    _items[i].SetActive(true);
                }
            }
        }
        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, _items.Length);
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }

        
    }
}