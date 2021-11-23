using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class RefillOilComponent : MonoBehaviour
    {
        public void RefillOil()
        {
            MainGOsUtils.GetGameSession().Data.Oil.Value = DefsFacade.I.Player.MaxOil;
        }
    }
}