using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        private readonly Dictionary<string, float> _dictionaryTimesHowPerksUsed = new Dictionary<string, float>();
        
        //public  Dictionary<string, float> DictionaryTimesHowPerksUsed => _dictionaryTimesHowPerksUsed;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        public List<string> Used => _data.Perks.Unlocked;
        public bool IsSuperThrowSupported => _data.Perks.Unlocked.Contains("super-throw");
        public bool IsDoubleJumpSupported => _data.Perks.Unlocked.Contains("double-jump");
        public bool IsForceShieldSupported => _data.Perks.Unlocked.Contains("force-shield");
        public bool IsSwordShieldSupported => _data.Perks.Unlocked.Contains("sword-shield");


        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            

            if (isEnoughResources)
            {

                if (id == "get-swords")
                {
                    if(!_data.Inventory.Add("Sword", (int) def.Cooldown))
                        return;
                }

                if (id == "get-potion-health")
                {
                    if(!_data.Inventory.Add("PotionHealth", (int) def.Cooldown))
                        return;
                }

                if (id == "get-big-potion-health")
                {
                    if(!_data.Inventory.Add("BigPotionHealth", (int) def.Cooldown))
                        return;
                } 
                else
                    _data.Perks.AddPerk(id);
                
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);

                OnChanged?.Invoke();
            }
        }
        
        
        
        public bool IsUnlockedForManage(string perkId)
        {
            if (perkId == "get-swords" || perkId == "get-potion-health" || perkId == "get-big-potion-health")
                return false;
            return _data.Perks.IsUnlocked(perkId);
        }
        public bool IsUnlockedForWidget(string perkId)
        {
            if (perkId == "get-swords" || perkId == "get-potion-health" || perkId == "get-big-potion-health")
                return true;
            return _data.Perks.IsUnlocked(perkId);
        }

        public Sprite PerkSprite(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            return def.Icon;
        }
        
        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void SetTimeHowPerkUsed(string id, float time)
        {
            _dictionaryTimesHowPerksUsed.Remove(id);
            _dictionaryTimesHowPerksUsed.Add(id, time);
        }

        public float GetTimeHowPerkUsed(string id)
        {
            float time = -10f;
            
            _dictionaryTimesHowPerksUsed.TryGetValue(id, out time);
            return time;
        }
        
        public void Dispose()
        {
            _trash.Dispose();
        }
        
        


        
    }
}