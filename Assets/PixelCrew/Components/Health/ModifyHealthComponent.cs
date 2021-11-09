using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;
namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        [SerializeField] private SourceDamage _sourceDamage;
        //private Hero _hero;

        private void Start()
        {
            //_hero = FindObjectOfType<Hero>();
        }
        public void ModifyHealth(GameObject go)
        {
            var hpDelta = (int) 0;
            switch (_sourceDamage)
            {
                case SourceDamage.HpDelta:
                    hpDelta = _hpDelta;
                    break;
                case SourceDamage.RangeDamageInGameSession:
                    var session = FindObjectOfType<GameSession>();
                    hpDelta = (int) (-1 * session.StatsModel.GetValue(StatId.RangeDamage));
                    Debug.Log(hpDelta);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var healthComponent = go.GetComponent<HealthComponent>();
            if (healthComponent != null)
                healthComponent.ModifyHealth(hpDelta);
        }
    }

    public enum SourceDamage
    {
        HpDelta,
        RangeDamageInGameSession
    }
}