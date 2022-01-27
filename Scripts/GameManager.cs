using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isClear;

    public bool IsClear
    {
        get { return this.isClear; }
        set { this.isClear = value; }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Random.InitState(System.DateTime.Now.Millisecond);
        isClear = false;
    }
}
