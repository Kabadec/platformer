using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;

        //private Hero _hero;

        private void Start()
        {
            //_hero = FindObjectOfType<Hero>();
        }
        public void ChangeHealth(GameObject go)
        {
            var healthComponent = go.GetComponent<HealthComponent>();
            if (healthComponent != null)
                healthComponent.ChangeHealth(_changeHealthValue);
        }
    }
}