using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLeakNoGC : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;

    private List<Sprite> _sprites = new List<Sprite>();
    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            _sprites.Add(Object.Instantiate(_sprite));
        }
    }
}
