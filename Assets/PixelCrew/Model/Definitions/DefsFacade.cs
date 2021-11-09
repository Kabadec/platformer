using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Repositories.Items;
using UnityEngine;
namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _itemses;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;


        public ItemsRepository Itemses => _itemses;
        public ThrowableRepository Throwable => _throwableItems;
        public PotionRepository Potions => _potions;
        public PerkRepository Perks => _perks;
        public PlayerDef Player => _player;


        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}