using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public bool Add(string id, int value)
        {
            if (value <= 0) return false;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return false;

            var item = GetItem(id);
            if (item == null || !itemDef.HasTag(ItemTag.Stackable))
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.Value += value;

            var trigger = true;
            if (itemDef.MaxCount != 0)
            {
                if (itemDef.MaxCount <= item.Value - value)
                    trigger = false;

                if (itemDef.MaxCount < item.Value)
                    item.Value = itemDef.MaxCount;
            }

            OnChanged?.Invoke(id, value);
            return trigger;
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);

            OnChanged?.Invoke(id, value);
        }
        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));
                if (isAllRequirementsMet)
                {
                    retValue.Add(item);
                }
                
            }
            return retValue.ToArray();
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }
            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                {
                    count += item.Value;
                }
            }
            return count;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}