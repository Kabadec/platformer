using System;
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
        public StringProperty InterfaceSelection = new StringProperty();

        private float _timeHowPerkUsed;
        
        public float TimeHowPerkUsed => _timeHowPerkUsed;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            
            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        public string Used => _data.Perks.Used.Value;
        public bool IsSuperThrowSupported => _data.Perks.Used.Value == "super-throw";
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump";
        public bool IsForceShieldSupported => _data.Perks.Used.Value == "force-shield";
        public bool IsSwordShieldSupported => _data.Perks.Used.Value == "sword-shield";


        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
                OnChanged?.Invoke();
            }
        }
        public void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }
        
        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public Sprite ActivePerkSprite()
        {
            var def = DefsFacade.I.Perks.Get(_data.Perks.Used.Value);
            return def.Icon;
        }
        
        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void SetTimeHowPerkUsed(float time)
        {
            _timeHowPerkUsed = time;
        }
        
        public void Dispose()
        {
            _trash.Dispose();
        }
        
        


        
    }
}