using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    private void Start()
    {
        Init();

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        bc2[0] = collisionObj[0].GetComponent<BoxCollider2D>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        bc2[1] = collisionObj[1].GetComponent<BoxCollider2D>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        coolCnt = 0f;
        coolTime = 5f;
        trackingDistance = 6f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        colOffset[0] = new Vector2(Mathf.Abs(bc2[0].offset.x), bc2[0].offset.y);
        colOffset[1] = new Vector2(Mathf.Abs(bc2[1].offset.x), bc2[1].offset.y);
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;
    }

    private void Update()
    {
        CoolTime();

        TrackingJudgment();

        if (!isCoolDown && eCollision.isInvasion)
        {
            StateChange(EnemyState.Attack);
        }

        switch (state)
        {
            case EnemyState.Wait:
                if (!isWait)
                {
                    isWait = true;
                    if (beforeState == EnemyState.Attack)
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
