using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class LerpAnimatedUtils
    {
        public static Coroutine LerpAnimated(this MonoBehaviour behaviour, float start, float end, float time,
            Action<float> onFrame)
        {
            return behaviour.StartCoroutine(Animate(start, end, time, onFrame));
        }

        private static IEnumerator Animate(float start, float end, float animationTime, Action<float> onFrame)
        {
            var time = 0f;
            onFrame(start);
            while (time < animationTime)
            {
                time += Time.deltaTime;
                var progress = time / animationTime;
                var value = Mathf.Lerp(start, end, progress);
                onFrame(value);

                yield return null;
            }

            onFrame(end);
        }
    }
}