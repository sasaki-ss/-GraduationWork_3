using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Zombie,
}

public class EventManager : MonoBehaviour
{

    private bool        isRunEvent;     //イベント実行フラグ
    private EventType   nowEventNum;    //現在のイベント番号

    private GameObject  zombie;     //ゾンビオブジェクト

    //初期化処理
    private void Start()
    {
        isRunEvent = false;
        zombie = (GameObject)Resources.Load("Zombie");
    }

    private void Update()
    {
        if (isRunEvent)
        {
            switch (nowEventNum)
            {
                case EventType.Zombie:

                    break;
            }
        }
    }

    public void StartRunEvent(EventType _eventType)
    {
        if (isRunEvent) return;

        isRunEvent = true;
        nowEventNum = _eventType;

        Debug.Log("イベント開始");
    }
}
