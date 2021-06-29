using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Creatures;
using PixelCrew.Creatures.Hero;
namespace PixelCrew.Components.Collectables
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
            _hero.AddCoins(_numCoins);
        }
    }
}