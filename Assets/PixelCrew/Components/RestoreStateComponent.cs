using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;
        
        private GameSession _session;
        
        private void Start()
        {
            _session = GameSession.Instance;
            var isDestroyed = _session.RestoreState(_id);
            if(isDestroyed)
                Destroy(gameObject);
        }

        
    }
}