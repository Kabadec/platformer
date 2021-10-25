using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils.Disposables
{
    public class ActionDisposable : IDisposable
    {
        private Action _onDispose;
        public ActionDisposable(Action onDispose)
        {
            _onDispose = onDispose;
        }
        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null;
        }
    }
}