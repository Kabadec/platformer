using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public struct DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        [SerializeField] private DialogType _type;
        public Sentence[] Sentences => _sentences;

        public DialogType Type => _type;

        
        public DialogData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<DialogData>(json);
        }
        
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _valued;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Side _side;

        public string Valued
        {
            get => _valued;
            set => _valued = value;
        }
        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }
        public Side Side
        {
            get => _side;
            set => _side = value;
        }
    }

    public enum Side
    {
        Left,
        Right
    }

    public enum DialogType
    {
        Simple,
        Personalized
    }
}