using System;
using System.Linq;
using UnityEngine;
namespace PixelCrew.Components.GoBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();
        }
        public void SpawnAll()
        {
            for (var i = 0; i < _spawners.Length; i++)
            {
                _spawners[i].Component.Spawn();
            }
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}