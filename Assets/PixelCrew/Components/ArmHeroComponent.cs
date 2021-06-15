using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{


    public class ArmHeroComponent : MonoBehaviour
    {
        private Hero _hero;


        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if(hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}