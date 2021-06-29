using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        //private Hero _hero;

        private void Start()
        {
            //_hero = FindObjectOfType<Hero>();
        }
        public void ModifyHealth(GameObject go)
        {
            var healthComponent = go.GetComponent<HealthComponent>();
            if (healthComponent != null)
                healthComponent.ChangeHealth(_hpDelta);
        }
    }
}