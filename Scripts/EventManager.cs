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

        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1.0f);

            GameObject zombieL = Instantiate(zombieObj,
                new Vector3(playerPos.x - 15.0f, 20.0f, 0f),
                Quaternion.identity);

            GameObject zombieR = Instantiate(zombieObj,
                new Vector3(playerPos.x + 15.0f, 20.0f, 0f),
                Quaternion.identity);

            zombieL.GetComponent<Zombie>().SetEnemyMovePattern(EnemyMovePattern.Event);
            zombieR.GetComponent<Zombie>().SetEnemyMovePattern(EnemyMovePattern.Event);
        }
    }
}
