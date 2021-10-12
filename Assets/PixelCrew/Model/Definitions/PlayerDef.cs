using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
    }
}