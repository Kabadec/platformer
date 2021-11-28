using Cinemachine;
using PixelCrew.Creatures.HeroAll;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            var hero = MainGOsUtils.GetMainHero();
            if(hero != null)
                vCamera.Follow = FindObjectOfType<Hero>().transform;
            else
                Debug.Log("hero is null!");
        }
    }
}