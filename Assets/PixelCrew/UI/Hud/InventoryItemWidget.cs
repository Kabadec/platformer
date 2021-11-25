using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud
{
    public class InventoryItemWidget : MonoBehaviour, IItemRenderer<InventoryItemData>
    {
        [SerializeField] protected Image _icon;
        [SerializeField] protected Text _value;
        
        protected readonly CompositeDisposable Trash = new CompositeDisposable();

        protected GameSession Session;

        protected virtual void Start()
        {
            Session = GameSession.Instance;
        }

        public virtual void SetData(InventoryItemData item, int index)
        {
            var def = DefsFacade.I.Itemses.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }
        
        protected virtual void OnDestroy()
        {
            Trash.Dispose();
        }
    }
}