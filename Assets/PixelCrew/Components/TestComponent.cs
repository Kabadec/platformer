using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixelCrew.Components
{
    public class TestComponent : MonoBehaviour
    {

        
            public enum PlayerStates
            {
                Heal,
                Damage
            }
            [Range(0, 20)] [SerializeField] private int _example;

            [SerializeField] private PlayerStates _state;

            public bool IsHealing => _state == PlayerStates.Heal;
            public bool IsDamaging => _state == PlayerStates.Damage;

            [ContextMenu("Print")]
            public void PrintState()
            {
                Debug.Log($"IsHealing: {IsHealing}");
                Debug.Log($"IsDamaging: {IsDamaging}");
            }

        }
    }