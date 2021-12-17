using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Zombie,
}

public class EventManager : MonoBehaviour
{

    private bool        isRunEvent;     //�C�x���g���s�t���O
    private EventType   nowEventNum;    //���݂̃C�x���g�ԍ�

    private GameObject  zombie;     //�]���r�I�u�W�F�N�g

    //����������
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

        Debug.Log("�C�x���g�J�n");
    }
}
