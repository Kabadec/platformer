using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        
        private const string SwordId = "Sword";

        private QuickInventoryItemWidget[] QuickItems => _session.QuickInventory.Items;
        

        public GameObject MouseOnMe
        {
            get => _session.QuickInventory.MouseOnMe;
            set => _session.QuickInventory.MouseOnMe = value;
        }
        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            var items = _container.gameObject.GetComponentsInChildren<QuickInventoryItemWidget>();
            var itemsData = _session.Data.Inventory.GetAll();
            for (int i = 0; i < items.Length; i++)
            {
                QuickItems[i] = items[i];
                SetQuickItem(itemsData[0], i, false);
            }
            _session.QuickInventory.SelectedIndex.Value = 1;
            _session.QuickInventory.SelectedIndex.Value = 0;

            foreach (var item in itemsData)
            {
                if (item.Id == SwordId)
                {
                    SetQuickItem(item, 0, true);
                }
            }
            foreach (var item in itemsData)
            {
                for (var i = 0; i < QuickItems.Length; i++)
                {
                    var itemDef = DefsFacade.I.Itemses.Get(item.Id);
                    var isUsable = itemDef.HasTag(ItemTag.Usable);
                    if(QuickItems[i].IsActive || !isUsable || item.Id == SwordId)
                        continue;
                    SetQuickItem(item, i, true);
                    break;
                }
            }

        }

        private void SetQuickItem(InventoryItemData data, int index, bool isActive)
        {
            QuickItems[index].SetData(data, index);
            QuickItems[index].SetActive(isActive);
        }
    }
}