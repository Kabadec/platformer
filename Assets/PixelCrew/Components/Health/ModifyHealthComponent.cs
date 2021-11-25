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

        //[SerializeField] private SourceDamage _sourceDamage;
        
        public void ModifyHealth(GameObject go)
        {
            // var hpDelta = (int) 0;
            // switch (_sourceDamage)
            // {
            //     case SourceDamage.HpDelta:
            //         hpDelta = _hpDelta;
            //         break;
            //     case SourceDamage.RangeDamageInGameSession:
            //         var session = GameSession.Instance;
            //         hpDelta = (int) (-1 * session.StatsModel.GetValue(StatId.RangeDamage));
            //         Debug.Log(hpDelta);
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
            // var healthComponent = go.GetComponent<HealthComponent>();
            // if (healthComponent != null)
            //     healthComponent.ModifyHealth(hpDelta);
            var healthComponent = go.GetComponent<HealthComponent>();
            if (healthComponent != null)
                healthComponent.ModifyHealth(_hpDelta);
        }
        public void SetHpDelta(int hpDelta)
        {
            _hpDelta = hpDelta;
        }
    }
    
    
    

    public enum SourceDamage
    {
        HpDelta,
        RangeDamageInGameSession
    }
}