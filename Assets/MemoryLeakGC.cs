using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryLeakGC : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;

    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            var instance = Instantiate(_sprite);
            Destroy(instance);
        }
    }
}
