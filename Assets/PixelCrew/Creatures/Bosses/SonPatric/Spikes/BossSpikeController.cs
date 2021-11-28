using System.Collections;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses.SonPatric.Spikes
{
    public class BossSpikeController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabSpikes;
        [SerializeField] private float _posY = 3f;
        [SerializeField] private float _delayBetweenSpikes = 0.3f;
        [SerializeField] private int _numSpikes = 10;
        

        private float _posX;
        private Coroutine _coroutine;

        [ContextMenu("Lets spike!")]
        public void LetsSpike()
        {
            _posX = transform.position.x;
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SpikeCoroutine());
        }

        private IEnumerator SpikeCoroutine()
        {
            for (var i = 0; i < _numSpikes; i++)
            {
                Pool.Instance.Get(_prefabSpikes, new Vector3((i + 1) + _posX, _posY, 0f), Vector3.one);
                Pool.Instance.Get(_prefabSpikes, new Vector3((-1 * (i + 1)) + _posX, _posY, 0f), Vector3.one);
                yield return new WaitForSeconds(_delayBetweenSpikes);
            }

            _coroutine = null;
        }
    }
}