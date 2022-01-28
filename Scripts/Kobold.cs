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

        atk = 50;
        hp = 100;
        def = 3;

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        aSrc = this.GetComponent<AudioSource>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        collisionObj[2].SetActive(false);

        coolCnt = 0f;
        coolTime = 2f;
        trackingDistance = 5f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        isMove = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;

        itemObj = (GameObject)Resources.Load("Item");

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

        if (!isAttack) collisionObj[2].SetActive(false);

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

                    rand = 1;

                    Inversion((Direction)rand);

                    isMove = true;
                }

                if(isMove)Move();
                break;
            case EnemyState.Attack:
                if (!isAttack)
                {
                    isAttack = true;
                    collisionObj[2].SetActive(true);
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
        //移動待機フラグがオフの場合
        if (!isMoveWait)
        {
            //壁がない場合
            if (!wallContact.getContact)
            {
                anim.SetBool("isMove", true);

                //左方向への移動
                if(dir == Direction.Left)
                {
                    this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
                }
                
                //右方向への移動
                if(dir == Direction.Right)
                {
                    this.transform.position += new Vector3(moveSpeed, 0f, 0f);
                }
            }
            //壁があった場合
            else
            {
                anim.SetBool("isMove", false);
            }

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
                Inversion(Direction.Right);
            }
            else if (dir == Direction.Right)
            {
                Inversion(Direction.Left);
            }

            nowMoveAmount = 0f;
            isMoveWait = true;
        }
    }

    void PlaySlashSound()
    {
        aSrc.PlayOneShot(ac[0]);
        aSrc.PlayOneShot(ac[ac.Length - 1]);
    }
}