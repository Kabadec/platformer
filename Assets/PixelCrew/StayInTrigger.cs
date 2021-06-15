using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    public class StayInTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_targetLayers == (_targetLayers | 1 << other.gameObject.layer))
            {
                other.transform.parent = transform;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_targetLayers == (_targetLayers | 1 << other.gameObject.layer))
            {
                other.transform.parent = null;
            }
        }
    }
}