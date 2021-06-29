using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}