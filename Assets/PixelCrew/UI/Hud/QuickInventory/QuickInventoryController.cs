using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
      
        public GameObject MouseOnMe
        {
            get => _session.QuickInventory.MouseOnMe;
            set => _session.QuickInventory.MouseOnMe = value;
        }
        private GameSession _session;

        private void Start()
        {
            _session = MainGOsUtils.GetGameSession();
            var items = _container.gameObject.GetComponentsInChildren<QuickInventoryItemWidget>();
            var firstItemData = _session.Data.Inventory.GetAll()[0];
            for (int i = 0; i < items.Length; i++)
            {
                _session.QuickInventory.Items[i] = items[i];
                _session.QuickInventory.Items[i].SetData(firstItemData, i);
                _session.QuickInventory.Items[i].SetActive(false);
                
            }
            _session.QuickInventory.SelectedIndex.Value = 1;
            _session.QuickInventory.SelectedIndex.Value = 0;
        }
    }
}