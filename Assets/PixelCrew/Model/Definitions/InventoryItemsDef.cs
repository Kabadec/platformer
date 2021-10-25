using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }
            return default;
        }

        public List<string> GetAllId()
        {
            var ids = new List<string>();
            foreach (var item in _items)
            {
                ids.Add(item.Id);
            }

            return ids;
        }
        public List<string> GetAllId(ItemTag tag)
        {
            var ids = new List<string>();
            foreach (var item in _items)
            {
                if (item.HasTag(tag))
                    ids.Add(item.Id);
            }

            return ids;
        }
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        //[SerializeField] private bool _isStuck;
        [SerializeField] private Sprite _icon;
        [Tooltip("Если 0, то не ограничено")] [SerializeField] private int _maxCount;
        [SerializeField] private ItemTag[] _tags;
        public string Id => _id;
        //public bool IsStuck => _isStuck;
        
        public int MaxCount => _maxCount;

        public bool IsVoid => string.IsNullOrEmpty(_id);
        public Sprite Icon => _icon;

        public bool HasTag(ItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }
}