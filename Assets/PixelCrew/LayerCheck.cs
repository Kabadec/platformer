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
    public bool IsTouchingPlatform;

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        if (other.gameObject.tag == "Platform")
        {
            IsTouchingPlatform = true;
        }
        else
        {
            IsTouchingPlatform = false;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        if (other.gameObject.tag == "Platform")
        {
            IsTouchingPlatform = true;
        }
        else
        {
            IsTouchingPlatform = false;
        }
    }
}
