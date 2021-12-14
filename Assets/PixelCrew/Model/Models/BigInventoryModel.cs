using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    public class BigInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        
        public InventoryItemData[] Inventory { get; private set; }

        public event Action OnChanged;

        public BigInventoryModel(PlayerData data)
        {
            _data = data;
            
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            
            _data.Inventory.OnChanged += OnChangedInventory;
            UpdateAllItems();
        }
        private void OnChangedInventory(string id, int value)
        {
            Inventory = _data.Inventory.GetAll();
            UpdateAllItems();
            OnChanged?.Invoke();
        }

        public void UpdateInventory()
        {
            OnChangedInventory("a", 1);
        }
        
        public InventoryItemData FoundItemByUniqueId(int uniqueId)
        {
            foreach (var item in Inventory)
            {
                if (item.UniqueId == uniqueId)
                    return item;
            }

            return default;
        }

        private void UpdateAllItems()
        {
            foreach (var item in Inventory)
            {
                if(item.PosBigInventory[0] == -1 ||item.PosBigInventory[0] == -1)
                    PutItemInFirstFreeSlot(item.UniqueId);
            }
        }
        

        private void PutItemInFirstFreeSlot(int uniqueId)
        {
            var lenght = 4;
            for (var i = 0; i < lenght; i++)
            {
                for (var j = 0; j < lenght; j++)
                {
                    if(!IsFreeSlot(i, j)) continue;
                    _data.Inventory.SetPosBigInventory(uniqueId, i, j);
                    return;
                }
            }
        }

        public bool PutItemInSomeSlot(int uniqueId, int posX, int posY)
        {
            if (!IsFreeSlot(posX, posY)) return false;
            _data.Inventory.SetPosBigInventory(uniqueId, posX, posY);
            return default;
        }

        private bool IsFreeSlot(int posX, int posY)
        {
            foreach (var item in Inventory)
            {
                if((int)item.PosBigInventory[0] != posX) continue;
                if((int)item.PosBigInventory[1] != posY) continue;
                return false;
            }

            return true;
        }


        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        
        
        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }

        
    }
}