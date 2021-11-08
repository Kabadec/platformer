using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId
    {
        [SerializeField] protected TDefType[] _collection;

        public TDefType Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return default;
            
            foreach (var itemDef in _collection)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }
            return default;
        }

        public TDefType[] All => new List<TDefType>(_collection).ToArray();
    }
}