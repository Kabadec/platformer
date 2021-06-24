using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Model;

namespace PixelCrew.Components
{
    public class ModifySwordsAmmoComponent : MonoBehaviour
    {
        [SerializeField] private int _swordsInHolderDelta;

        public void ModifySwordsInHolder(GameObject go)
        {
            var swordsInHolderComponent = go.GetComponent<SwordsAmmoComponent>();
            if (swordsInHolderComponent != null)
                swordsInHolderComponent.ModifySwordsAmmo(_swordsInHolderDelta);
        }

    }
}