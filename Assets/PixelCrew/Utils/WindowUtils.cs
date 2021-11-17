using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);

            var canvas = MainGOsUtils.GetMainCanvas();
            Object.Instantiate(window, canvas.transform);
        }
    }
}