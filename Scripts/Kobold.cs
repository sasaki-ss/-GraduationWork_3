using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : Enemy
{
    private float       moveWaitTime;   //�ړ���ҋ@���鎞��
    private float       moveWaitCnt;    //�ҋ@�J�E���g
    private float       maxMoveAmount;  //�ړ�����ő��
    private float       nowMoveAmount;  //���݂̈ړ���
    private bool        isMove;         //�ړ��t���O
    private bool        isMoveWait;     //�ړ��ҋ@�t���O

    //����������
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

    //�X�V����
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

            //�ړ��ʂ����Z
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

        //���݂̈ړ��ʂ��ő�ʂ𒴂���Ƃ������𔽓]������
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