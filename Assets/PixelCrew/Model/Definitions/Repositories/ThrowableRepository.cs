using System;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{  
    [CreateAssetMenu(menuName = "Defs/Repositories/Throwable", fileName = "Throwable")]
    public class ThrowableRepository : DefRepository<ThrowableDef>
    {
#if UNITY_EDITOR
        public ThrowableDef[] ItemsForEditor => _collection;
#endif
    }
    
    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;
        
        public string Id => _id;
        public GameObject Projectile => _projectile;
    }
}