using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        protected readonly List<TItemType> CreatedItem = new List<TItemType>();
        private readonly TItemType _prefab;
        private readonly Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TDataType> data)
        {
            // create required items
            for (var i = CreatedItem.Count; i < data.Count(); i++)
            {
                var item = Object.Instantiate(_prefab, _container);
                CreatedItem.Add(item);
            }

            // update data and activate
            for (var i = 0; i < data.Count; i++)
            {
                CreatedItem[i].SetData(data[i], i);
                CreatedItem[i].gameObject.SetActive(true);
            }

            // hide unused items
            for (var i = data.Count; i < CreatedItem.Count; i++)
            {
                CreatedItem[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<in TDataType>
    {
        void SetData(TDataType data, int index);
    }
}