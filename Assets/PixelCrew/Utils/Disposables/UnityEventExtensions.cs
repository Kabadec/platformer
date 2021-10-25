using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Utils.Disposables
{
    public static class UnityEventExtensions
    {

        public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call)
        {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
        public static IDisposable Subscribe<TType>(this UnityEvent<TType> unityEvent, UnityAction<TType> call)
        {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
    }
}
