using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Candle
{
    public class FillOilComponent : MonoBehaviour
    {
        public void FillOil()
        {
            MainGOsUtils.GetGameSession().Data.Oil.Value = DefsFacade.I.Player.MaxOil;
        }
    }
}