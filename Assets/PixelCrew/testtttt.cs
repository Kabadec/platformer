using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtttt : MonoBehaviour
{
    [SerializeField] private GameObject go;
    private Animator _animator;
    private void Awake()
    {
        _animator = go.GetComponent<Animator>();

    }
    private void Update()
    {
        if (_animator == null)
            Debug.Log("animator is null");
        else
            Debug.Log("animator accept");
    }
}
