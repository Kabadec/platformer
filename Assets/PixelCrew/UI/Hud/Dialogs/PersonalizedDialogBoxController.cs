using PixelCrew.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class PersonalizedDialogBoxController : DialogBoxController
    {
        [SerializeField] private DialogContent _right;

        protected override DialogContent CurrentContent => CurrentSentence.Side == Side.Left ? _content : _right;

        protected override void OnStartDialogAnimation()
        {
            SetActiveRightOrLeft();
            
            base.OnStartDialogAnimation();
        }

        public override void ShowDialog(DialogData data, UnityEvent onComplete)
        {
            base.ShowDialog(data, onComplete);
            SetActiveRightOrLeft();
        }

        private void SetActiveRightOrLeft()
        {
            _right.gameObject.SetActive(CurrentSentence.Side == Side.Right);
            _content.gameObject.SetActive(CurrentSentence.Side == Side.Left);
        }
        
    }
}