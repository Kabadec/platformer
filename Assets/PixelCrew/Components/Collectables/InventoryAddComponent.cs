using UnityEngine;
using PixelCrew.Utils;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Repositories.Items;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;
        [SerializeField] private EnterEvent _added;
        [SerializeField] private EnterEvent _notAdded;


        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>();
            if (hero != null)
            {
                if (hero.AddInInventory(_id, _count))
                    _added?.Invoke(go);
                else
                    _notAdded?.Invoke(go);
            }
        }
    }
}