using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFPS : MonoBehaviour
{
    [SerializeField] private int _fps;

    [ContextMenu("Set FPS")]
    public void SetFps()
    {
        Application.targetFrameRate = _fps;
    }
}
