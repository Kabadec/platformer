using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public static class GameObjectExtension
    {
        public static float Area(this Transform transform)
        {
            return transform.localScale.x * transform.localScale.y;
        }
    }
}