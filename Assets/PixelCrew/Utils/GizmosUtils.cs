using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GizmosUtils
    {
        public static void DrawBounds(Bounds bounds, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(bounds.min, new Vector3(bounds.min.x, bounds.max.y));
            Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), bounds.max);
            Gizmos.DrawLine(bounds.max, new Vector3(bounds.max.x, bounds.min.y));
            Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y), bounds.min);

            Gizmos.color = prevColor;
        }
    }
}