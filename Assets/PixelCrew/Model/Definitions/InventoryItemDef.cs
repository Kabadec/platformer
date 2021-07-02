using System.Collections;
using System.Collections.Generic;
using System;
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
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isStuck;
        [Tooltip("Если 0, то не ограничено")] [SerializeField] private int _maxCount;
        public string Id => _id;
        public bool IsStuck => _isStuck;
        public int MaxCount => _maxCount;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}