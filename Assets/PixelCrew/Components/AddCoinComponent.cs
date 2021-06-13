using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class AddCoinComponent : MonoBehaviour
    {
        [SerializeField] private int _numCoins;
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }
        public void Take()
        {
            _hero.TakeCoin(_numCoins);
        }
    }
}