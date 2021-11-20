using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Hud.Dialogs;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;
        [SerializeField] private UnityEvent _onComplete;

        private DialogBoxController _dialogBox;
        
        public void Show()
        {
            _dialogBox = FindDialogController();
            
            if (_mode == Mode.Bound)
                _dialogBox.ShowDialog(LocalizedBound(), _onComplete);
            else
                _dialogBox.ShowDialog(Data, _onComplete);
        }

        private DialogData LocalizedBound()
        {
            var localizedBound = _bound.Clone();
            var i = 0;
            foreach (var key in _bound.Sentences)
            {
                localizedBound.Sentences[i].Valued = LocalizationManager.I.Localize(key.Valued);
                i++;
            }
            return localizedBound;
        }

        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject controllerGo = null;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                default:
                    throw new ArgumentException("Undefined dialog type");
            }

            return controllerGo.GetComponent<DialogBoxController>();
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }
        
        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public enum Mode
        {
            Bound,
            External
        }
    }
}