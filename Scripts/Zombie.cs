using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    private void Start()
    {
        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        atk = 10;
        hp = 30;
        def = 5;

        coolCnt = 0f;
        coolTime = 1f;
        trackingDistance = 10f;
        moveSpeed = 0.01f;
        waitTime = 5f;
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        dir = Direction.Left;
    }

    private void Update()
    {
        CoolTime();

        if (eMovePattern != EnemyMovePattern.Event) TrackingJudgment();

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
                Move();
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

    private void Move()
    {
        switch (eMovePattern)
        {
            case EnemyMovePattern.Normal:
                break;
            case EnemyMovePattern.Event:
                if(this.transform.position.x > player.transform.position.x)
                {
                    this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
                    Inversion(Direction.Left);
                }

                if(this.transform.position.x < player.transform.position.x)
                {
                    this.transform.position += new Vector3(moveSpeed, 0f, 0f);
                    Inversion(Direction.Right);
                }

                break;
        }
    }
}
