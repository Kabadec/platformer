using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Bosses.SonPatric.Spikes
{
    public class BossSpike : MonoBehaviour
    {
        [SerializeField] private GameObject _spike;
        [SerializeField] private float _heightSpike;
        [SerializeField] private float _speedUp = 1f;
        [SerializeField] private float _speedDown = 0.5f;
        [SerializeField] private UnityEvent _onEnd;

        [SerializeField] private bool _startSpike = false;

        private bool _isUp = true;

        private void Start()
        {
            ResetSpike();
        }
        
        private void Update()
        {
            if(!_startSpike)
                return;
            
            if (_isUp)
            {
                var spikePos = _spike.transform.localPosition;
                spikePos.y += _speedUp * Time.deltaTime;
                _spike.transform.localPosition = spikePos;
                if (spikePos.y >= _heightSpike)
                    _isUp = false;
            }
            else
            {
                var spikePos = _spike.transform.localPosition;
                spikePos.y -= _speedDown * Time.deltaTime;
                _spike.transform.localPosition = spikePos;
                if (spikePos.y <= 0f)
                {
                    _startSpike = false;
                    _onEnd?.Invoke();
                }
            }
        }

        [ContextMenu("Reset Spike")]
        public void ResetSpike()
        {
            _startSpike = true;
            _isUp = true;
            
            var transformPosition = _spike.transform.localPosition;
            transformPosition.y = 0;
            _spike.transform.localPosition = transformPosition;
        }
    }
}