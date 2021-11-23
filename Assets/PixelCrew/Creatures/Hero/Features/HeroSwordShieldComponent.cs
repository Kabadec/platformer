using UnityEngine;

namespace PixelCrew.Creatures.Hero.Features
{
    public class HeroSwordShieldComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _swordShieldPrefab;
        private GameObject _swordShieldGO;
        
        public void Use()
        {
            if(_swordShieldGO != null) Destroy(_swordShieldGO);
            _swordShieldGO = Instantiate(_swordShieldPrefab, gameObject.transform);
        }
    }
}