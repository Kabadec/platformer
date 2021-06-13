using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
             return layer == (layer | 1 << go.layer);           
        }
    }
}