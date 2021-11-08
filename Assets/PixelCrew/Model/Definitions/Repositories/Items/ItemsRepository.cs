using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories.Items
{
    [CreateAssetMenu(menuName = "Defs/Repositories/Items", fileName = "Items")]
    public class ItemsRepository : DefRepository<ItemDef>
    {
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif
    }

    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [Tooltip("Если 0, то не ограничено")] [SerializeField] private int _maxCount;
        [SerializeField] private ItemTag[] _tags;
        
        public string Id => _id;
        public int MaxCount => _maxCount;
        public bool IsVoid => string.IsNullOrEmpty(_id);
        public Sprite Icon => _icon;

        
        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
    }
}