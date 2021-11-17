using System.Collections;
using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _isSaveParent = false;
        [SerializeField] private bool _invertXScale;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            SpawnInstance();
        }

        public GameObject SpawnInstance()
        {
            var instantiate = CreatingGameObject();
            if (_isSaveParent)
            {
                instantiate.transform.parent = this.transform.parent;
            }

            return instantiate;
        }
        public void Spawn(GameObject parentGo)
        {
            var instantiate = CreatingGameObject();
            instantiate.transform.parent = parentGo.transform;
        }
        private GameObject CreatingGameObject()
        {
            var instantiate = SpawnUtils.Spawn(_prefab, _target.position);

            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instantiate.transform.localScale = scale;

            return instantiate;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}