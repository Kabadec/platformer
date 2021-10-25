using System;
using System.Linq;
using UnityEngine;
namespace PixelCrew.Model.Definitions
{  
    [CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
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
        public ThrowableDef[] ItemsForEditor => _items;
#endif
    }
    [Serializable]
    public struct ThrowableDef
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;
        
        public string Id => _id;
        public GameObject Projectile => _projectile;
        
        
        //[SerializeField] private Sprite _icon;
        //[Tooltip("Если 0, то не ограничено")] [SerializeField] private int _maxCount;
        //[SerializeField] private ItemTag[] _tags;
        //public string Id => _id;
        
        //public int MaxCount => _maxCount;

        //public bool IsVoid => string.IsNullOrEmpty(_id);
        //public Sprite Icon => _icon;

        //public bool HasTag(ItemTag tag)
        //{
        //    return _tags.Contains(tag);
        //}
    }
}