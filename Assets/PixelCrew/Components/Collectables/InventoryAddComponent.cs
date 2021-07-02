﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;

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
            var hero = go.GetComponent<Hero>();
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