using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvases = Object.FindObjectsOfType<Canvas>();
            foreach (var canvas in canvases)
            {
                if (!canvas.CompareTag("MainCanvas")) continue;
                Object.Instantiate(window, canvas.transform);
                return;
            }
        }
    }
}