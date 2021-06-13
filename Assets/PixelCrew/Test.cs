using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private string _text1;
    [SerializeField] private string _text2;
    [SerializeField] private string _text3;

    public void SayText1()
    {
        Debug.Log(_text1);
    }
    public void SayText2()
    {
        Debug.Log(_text2);
    }
    public void SayText3()
    {
        Debug.Log(_text3);
    }
}
