using PixelCrew.Creatures.Bosses.Patric.Bombs;
using UnityEngine;

namespace PixelCrew.Creatures.Bosses.Patric
{
    public class BossBombingState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<BombsController>();
            spawner.StartBombing();
        }
    }
}