using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class AddRandomForceComponent : MonoBehaviour
{
    [SerializeField] bool _enableCasualForce = true;
    [SerializeField] float _casualForce = 3;
    [Space]
    [Header("X force:")]
    [SerializeField] private float _minXForce = -3;
    [SerializeField] private float _maxXForce = 3;
    [Space]
    [Header("Y force:")]
    [SerializeField] private float _minYForce = -3;
    [SerializeField] private float _maxYForce = 3;


    private Rigidbody2D _rigidbody;

    private void OnEnable()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        AddRandomForce();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void AddRandomForce()
    {
        if (_enableCasualForce)
        {
            _rigidbody.AddForce(new Vector2(Random.Range(-_casualForce, _casualForce), Random.Range(-_casualForce, _casualForce)), ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody.AddForce(new Vector2(Random.Range(_minXForce, _maxXForce), Random.Range(_minYForce, _maxYForce)), ForceMode2D.Impulse);

        }
    }
}
