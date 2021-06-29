using System;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;

namespace PixelCrew.Components.ColliderBased
{
    public class StayTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject);
            }
        }



    }
}


