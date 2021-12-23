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

    private GameObject  zombieObj;     //ゾンビオブジェクト

    //初期化処理
    private void Start()
    {
        isRunEvent = false;
        zombieObj = (GameObject)Resources.Load("Zombie");
    }

    private void Update()
    {
        if (isRunEvent)
        {

        }
    }

    public void StartRunEvent(EventType _eventType)
    {
        if (isRunEvent) return;

        isRunEvent = true;
        nowEventNum = _eventType;

        switch (nowEventNum)
        {
            case EventType.Zombie:
                StartCoroutine(ZombieEvent());
                break;
        }
    }

    private IEnumerator ZombieEvent()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(1.2f);

            if(i >= 0 && i < 15)
            {
                ZombieCreate(new Vector3(playerPos.x - 12.0f, 10.0f, 0f), Quaternion.identity);
            }

            if(i >= 15 && i < 30)
            {
                ZombieCreate(new Vector3(playerPos.x + 12.0f, 10.0f, 0f), Quaternion.identity);
            }

            if(i >= 30)
            {
                ZombieCreate(new Vector3(playerPos.x - 12.0f, 10.0f, 0f), Quaternion.identity);
                ZombieCreate(new Vector3(playerPos.x + 12.0f, 10.0f, 0f), Quaternion.identity);
            }
        }
    }

    private void ZombieCreate(Vector3 _pos, Quaternion _rot)
    {
        GameObject zombie = Instantiate(zombieObj, _pos, _rot);
        zombie.GetComponent<Zombie>().SetEnemyMovePattern(EnemyMovePattern.Event);
    }
}
