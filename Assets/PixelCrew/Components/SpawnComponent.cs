using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
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
            var instantiate = Creating();
            if (_isSaveParent)
            {
                instantiate.transform.parent = this.transform.parent;
            }
        }
        public void Spawn(GameObject parentGo)
        {
            var instantiate = Creating();
            instantiate.transform.parent = parentGo.transform;
        }
        private GameObject Creating()
        {
            var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);

            var scale = _target.lossyScale;
            scale.x *= _invertXScale ? -1 : 1;
            instantiate.transform.localScale = scale;

            return instantiate;
        }

    }
}