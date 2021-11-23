using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private float _maxOil;
        [SerializeField] private StatDef[] _stats;

        public float MaxOil => _maxOil;
        
        public StatDef[] Stats => _stats;


        public StatDef GetStat(StatId id)
        {
            foreach (var statDef in _stats)
            {
                if (statDef.ID == id)
                    return statDef;
            }

            return default;
            // _stats.FirstOrDefault(x => x.ID == id);
        }
            
    }
}