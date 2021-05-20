using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    private Collider2D _collider;

    
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public bool IsTouchingLayer;

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);

    }
}
