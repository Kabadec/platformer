using System.Collections.Generic;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Utils.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new Dictionary<int, Queue<PoolItem>>();

        private static Pool _instance;

        public static Pool Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("###_MAIN_POOL_###");
                    _instance = go.AddComponent<Pool>();
                }
                return _instance;
            }
        }

        public GameObject Get(GameObject go, Vector3 position, Vector3 scale)
        {
            var id = go.GetInstanceID();
            var queue = RequireQueue(id);

            if (queue.Count > 0)
            {
                var pooledItem = queue.Dequeue();
                var pooledItemTransform = pooledItem.transform;
                
                
                pooledItemTransform.position = position;
                pooledItemTransform.localScale = scale;
                pooledItem.gameObject.SetActive(true);
                pooledItem.Restart();
                
                return pooledItem.gameObject;
            }

            var instance = SpawnUtils.Spawn(go, position, gameObject.name);
            instance.transform.localScale = scale;
            
            var poolItem = instance.GetComponent<PoolItem>();
            poolItem.Retain(id, this);

            return instance;
        }


        private Queue<PoolItem> RequireQueue(int id)
        {
            if (!_items.TryGetValue(id, out var queue))
            {
                queue = new Queue<PoolItem>();
                _items.Add(id, queue);
            }

            return queue;
        }
        public void Release(int id, PoolItem poolItem)
        {
            var queue = RequireQueue(id);
            queue.Enqueue(poolItem);
            
            poolItem.gameObject.SetActive(false);
        }
    }
}