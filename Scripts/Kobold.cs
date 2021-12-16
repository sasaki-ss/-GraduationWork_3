using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : Enemy
{
    private float       moveWaitTime;   //移動を待機する時間
    private float       moveWaitCnt;    //待機カウント
    private float       maxMoveAmount;  //移動する最大量
    private float       nowMoveAmount;  //現在の移動量
    private bool        isMove;         //移動フラグ
    private bool        isMoveWait;     //移動待機フラグ

    //初期化処理
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
        coolTime = 2f;
        trackingDistance = 5f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        colOffset[0] = new Vector2(Mathf.Abs(bc2[0].offset.x), bc2[0].offset.y);
        colOffset[1] = new Vector2(Mathf.Abs(bc2[1].offset.x), bc2[1].offset.y);
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        isMove = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;

        moveWaitTime = 1f;
        moveWaitCnt = 0f;
        maxMoveAmount = 5f;
        nowMoveAmount = 0f;
        isMoveWait = false;
    }

    //更新処理
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
                isMove = false;
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
                if (!isMove)
                {
                    int rand = (int)Random.Range(0, 2f);

                    if (rand == (int)Direction.Left)
                    {
                        dir = Direction.Left;
                    }

                    if (rand == (int)Direction.Right)
                    {
                        dir = Direction.Right;
                    }

                    isMove = true;
                }

                if(isMove)Move();
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
        if (!isMoveWait)
        {
            anim.SetBool("isMove", true);

            MoveProc();

            //移動量を加算
            nowMoveAmount += moveSpeed;
        }
        else
        {
            anim.SetBool("isMove", false);

            if(moveWaitCnt >= moveWaitTime)
            {
                isMoveWait = false;
                moveWaitCnt = 0f;
            }
            else
            {
                moveWaitCnt += Time.deltaTime;
            }
        }

        //現在の移動量が最大量を超えるとき方向を反転させる
        if (nowMoveAmount >= maxMoveAmount)
        {
            if (dir == Direction.Left)
            {
                dir = Direction.Right;
            }
            else if (dir == Direction.Right)
            {
                dir = Direction.Left;
            }

            nowMoveAmount = 0f;
            isMoveWait = true;
        }
    }
}