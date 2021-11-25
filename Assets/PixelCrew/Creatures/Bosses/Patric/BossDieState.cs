using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses.Patric
{
    public class BossDieState: StateMachineBehaviour
    {
        [ColorUsage(true, true)]
        [SerializeField] private Color _stageColor;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var changeLight = animator.GetComponent<ChangeLightsComponent>();
            changeLight.SetColor(_stageColor);
        }

    }
}