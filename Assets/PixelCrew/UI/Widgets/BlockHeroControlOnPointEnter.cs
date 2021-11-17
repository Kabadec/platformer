using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Widgets
{
    public class BlockHeroControlOnPointEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            MainGOsUtils.GetMainHero().CanControlHero = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MainGOsUtils.GetMainHero().CanControlHero = true;
        }
    }
}