using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Hud.Dialogs;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;
        
        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();
            var localizedData = new string[_bound.Sentences.Length];
            if (_mode == Mode.Bound)
            {
                var i = 0;
                foreach (var key in _bound.Sentences)
                {
                    localizedData[i] = LocalizationManager.I.Localize(key);
                    i++;
                }
                DialogData data = new DialogData(localizedData);
                _dialogBox.ShowDialog(data);
            }
            else
                _dialogBox.ShowDialog(Data);
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
                        break;
                    case Mode.External:
                        return _external.Data;
                        break;
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