using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Items;
using Random = UnityEngine.Random;

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

            var itemDef = DefsFacade.I.Itemses.Get(id);
            if (itemDef.IsVoid) return false;

            var item = GetItem(id);
            if (item == null || !itemDef.HasTag(ItemTag.Stackable))
            {
                item = new InventoryItemData(id);
                item.UniqueId = CreateUniqueId();
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
            var itemDef = DefsFacade.I.Itemses.Get(id);
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
                var itemDef = DefsFacade.I.Itemses.Get(item.Id);
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
        public InventoryItemData GetItemByUniqueId(int uniqueId)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.UniqueId == uniqueId)
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

        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();
            foreach (var item in items)
            {
                if (joined.ContainsKey(item.ItemId))
                    joined[item.ItemId] += item.Count;
                else
                {
                    joined.Add(item.ItemId, item.Count);
                }
            }

            foreach (var kvp in joined)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value) return false;
            }
            return true;
        }

        private int CreateUniqueId()
        {
            var uniqueId = 0;
            while (true)
            {
                uniqueId = (int)(Random.value * 10000);
                foreach (var item in _inventory)
                {
                    if(uniqueId == -1) continue;
                    if (item.UniqueId == uniqueId) uniqueId = -1;
                }

                if (uniqueId != -1) return uniqueId;
            }
        }

        public void SetPosBigInventory(int uniqueId, int posX, int posY)
        {
            foreach (var item in _inventory)
            {
                if(item.UniqueId != uniqueId) continue;
                item.PosBigInventory[0] = posX;
                item.PosBigInventory[1] = posY;
                return;
            }
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;
        [Space]
        public int UniqueId;
        public int[] PosBigInventory = {-1, -1};

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}