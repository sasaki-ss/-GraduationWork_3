using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : Enemy
{
    private void Start()
    {
        Init();

        coolCnt = 0f;
        coolTime = 3f;
        trackingDistance = 5f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        colOffset[0] = new Vector2(0.3f, -0.245f);
        colOffset[1] = new Vector2(0.26f, -0.26f);
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        bc2[0] = collisionObj[0].GetComponent<BoxCollider2D>();
        eCollision[0] = collisionObj[0].GetComponent<EnemyCollision>();
        bc2[1] = collisionObj[1].GetComponent<BoxCollider2D>();
        eCollision[1] = collisionObj[1].GetComponent<EnemyCollision>();
    }

    private void Update()
    {
        CoolTime();

        TrackingJudgment();

        if (!isCoolDown && eCollision[0].isInvasion)
        {
            StateChange(EnemyState.Attack);
        }

        switch (state)
        {
            case EnemyState.Wait:
                if (!isWait)
                {
                    isWait = true;
                    if(beforeState == EnemyState.Attack)
                    {
                        StartCoroutine(Wait(waitTime / 2));
                    }
                    else
                    {
                        StartCoroutine(Wait(waitTime));
                    }
                }
                break;
            case EnemyState.Move:
                //Move();
                break;
            case EnemyState.Attack:
                if (!isAttack)
                {
                    isAttack = true;
                    StartCoroutine(Attack());
                }
                break;
            case EnemyState.Tracking:
                Tracking();
                break;
        }
    }
}