using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Zombie,
    Dragon,
}

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private int         defeatNum;      //���j��
    [SerializeField]
    private int         maxDefeatNum;   //�ő�̌��j��
    private bool        isRunEvent;     //�C�x���g���s�t���O
    private EventType   nowEventNum;    //���݂̃C�x���g�ԍ�

    private GameObject  zombieObj;     //�]���r�I�u�W�F�N�g

    //����������
    private void Start()
    {
        defeatNum = 0;
        maxDefeatNum = 0;
        isRunEvent = false;
        zombieObj = (GameObject)Resources.Load("Zombie");
    }

    private void Update()
    {
        if (isRunEvent)
        {
            if(nowEventNum == EventType.Zombie)
            {
                if(defeatNum >= maxDefeatNum)
                {
                    isRunEvent = false;
                    defeatNum = 0;
                    maxDefeatNum = 0;
                    GameObject.Find("Main Camera").GetComponent<
                        FollowCamera>().SetMoveFlg = true;
                }
            }
        }
    }

    public void Defeat()
    {
        defeatNum++;
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
            case EventType.Dragon:
                DragonEvent();
                break;
        }
    }

    private IEnumerator ZombieEvent()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;

        int firstPhaseNum = 15;
        int secondPhaseNum = 30;
        int finalPhaseNum = 45;

        maxDefeatNum = firstPhaseNum + (secondPhaseNum - firstPhaseNum)
            + ((finalPhaseNum - secondPhaseNum) * 2);


        for (int i = 0; i < finalPhaseNum; i++)
        {
            yield return new WaitForSeconds(0.5f);

            if(i >= 0 && i < firstPhaseNum)
            {
                ZombieCreate(new Vector3(playerPos.x - 12.0f, 10.0f, 0f), Quaternion.identity);
            }

            if(i >= firstPhaseNum && i < secondPhaseNum)
            {
                ZombieCreate(new Vector3(playerPos.x + 12.0f, 10.0f, 0f), Quaternion.identity);
            }

            if (i == secondPhaseNum) yield return new WaitForSeconds(2.0f);

            if (i >= secondPhaseNum)
            {
                ZombieCreate(new Vector3(playerPos.x - 12.0f, 10.0f, 0f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                ZombieCreate(new Vector3(playerPos.x + 12.0f, 10.0f, 0f), Quaternion.identity);
            }
        }
    }

    private void DragonEvent()
    {
        GameObject.Find("Dragon").GetComponent<Dragon>().IsActive = true;
    }

    private void ZombieCreate(Vector3 _pos, Quaternion _rot)
    {
        GameObject zombie = Instantiate(zombieObj, _pos, _rot);
        zombie.GetComponent<Zombie>().SetEnemyMovePattern(EnemyMovePattern.Event);
        zombie.name = "Zombie";
        zombie.AddComponent<EventEnemy>();
    }
}
