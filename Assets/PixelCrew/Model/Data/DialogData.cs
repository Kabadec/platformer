using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private string[] _sentences;
        public string[] Sentences => _sentences;

        public DialogData()
        {
            
        }

        public DialogData(string[] data)
        {
            _sentences = data;
        }
    }
}