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

    //�X�V����
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
        //�ړ��ҋ@�t���O���I�t�̏ꍇ
        if (!isMoveWait)
        {
            //�ǂ��Ȃ��ꍇ
            if (!wallContact.getContact)
            {
                anim.SetBool("isMove", true);

                //�������ւ̈ړ�
                if(dir == Direction.Left)
                {
                    this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
                }
                
                //�E�����ւ̈ړ�
                if(dir == Direction.Right)
                {
                    this.transform.position += new Vector3(moveSpeed, 0f, 0f);
                }
            }
            //�ǂ��������ꍇ
            else
            {
                anim.SetBool("isMove", false);
            }

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