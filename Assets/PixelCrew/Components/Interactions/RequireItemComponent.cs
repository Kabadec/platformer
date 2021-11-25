using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model.Definitions;
using PixelCrew.Model;
using UnityEngine.Events;
using PixelCrew.Model.Data;


namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [Space]
        [Space]
        [SerializeField] private bool _removeAfterUse;


        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;
        public void Check()
        {
            var session = GameSession.Instance;

            var areAllRequirementsMet = true;
            foreach (var item in _required)
            {
                var numItems = session.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                    areAllRequirementsMet = false;
            }
            if (areAllRequirementsMet)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                        session.Data.Inventory.Remove(item.Id, item.Value);
                }
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}