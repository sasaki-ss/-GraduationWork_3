using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : Enemy
{
    private void Start()
    {
        coolCnt = 0f;
        coolTime = 3f;
        trackingDistance = 5f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        colOffset = new Vector3(0.3f, -0.245f);
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        bc2 = collisionObj.GetComponent<BoxCollider2D>();
        eCollision = collisionObj.GetComponent<EnemyCollision>();
    }

    private void Update()
    {
        CoolTime();

        TrackingJudgment();

        if (!isCoolDown && eCollision.isAttack)
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