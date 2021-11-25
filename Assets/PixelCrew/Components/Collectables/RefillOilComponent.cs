using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class RefillOilComponent : MonoBehaviour
    {
        public void RefillOil()
        {
            GameSession.Instance.Data.Oil.Value = DefsFacade.I.Player.MaxOil;
        }
    }
}