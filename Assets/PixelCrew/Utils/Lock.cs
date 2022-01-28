using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace PixelCrew.Utils
{
    public class Lock
    {
        private readonly List<Object> _retained = new List<object>();

        public void Retain(object item)
        {
            _retained.Add(item);
        }

        public void Release(object item)
        {
            var isRemoved = _retained.Remove(item);
            if(!isRemoved)
                Debug.LogError($"Object {item} cannot removed!");
        }

        public bool IsLocked => _retained.Count > 0;
    }
}