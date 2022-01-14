using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviour

public class Dragon : Enemy
{
    private GameObject  fireBall;

    private float       disTwoPoints;
    private float       nextAttackTime;
    private float       attackTimeCnt;

    [SerializeField]
    private int         rashMovePhase;
    private float       rashMoveSpeed;
    [SerializeField]
    private float       rashMoveDis;
    private bool        isRushAttack;
    private bool        isRashMoveLeft;


    private void Start()
    {
        atk = 50;
        hp = 10000;

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        coolCnt = 0f;
        coolTime = 2f;
        trackingDistance = 5f;
        moveSpeed = 0.03f;
        waitTime = 3f;
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        dir = Direction.Left;
        beforeState = EnemyState.Wait;
        state = EnemyState.Wait;

        fireBall = (GameObject)Resources.Load("FireBall");
        disTwoPoints = 0;
        nextAttackTime = 5f;
        attackTimeCnt = 0f;
        rashMovePhase = 0;
        rashMoveSpeed = 0.1f;
        rashMoveDis = 0;
        isRushAttack = false;
        isRashMoveLeft = false;
    }

    private void Update()
    {
        disTwoPoints = player.transform.position.x - this.transform.position.x;

        if (!isAttack) AttackStateChange();

        //CoolTime();

        //TrackingJudgment();

        //if (!isCoolDown && eCollision.isInvasion)
        //{
        //    StateChange(EnemyState.Attack);
        //}

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
                    AttackProc();
                }
                if (isRushAttack) RushAttack();
                break;
            case EnemyState.Tracking:

                break;
        }
    }

    //移動処理
    private void Move()
    {
        anim.SetBool("isMove", true);

        if (!wallContact.getContact)
        {
            //disTwoPointsが-なら左へ、+なら右へ移動、0の場合移動しない
            if (disTwoPoints < 0)
            {
                this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
                Inversion(Direction.Left);
            }
            else if (disTwoPoints > 0)
            {
                this.transform.position += new Vector3(moveSpeed, 0f, 0f);
                Inversion(Direction.Right);
            }
            else
            {
                anim.SetBool("isMove", false);
            }
        }
        else
        {
            anim.SetBool("isMove", false);
            return;
        }
    }

    private void AttackProc()
    {
        //
        if (eCollision.isInvasion)
        {
            isRushAttack = true;
            return;
        }
        //攻撃判定外の場合ブレス攻撃
        else
        {
            StartCoroutine(Attack());
        }
    }

    private void RushAttack()
    {
        if(rashMovePhase == 0 || rashMovePhase == 2)
        {
            if(disTwoPoints < 0)
            {
                isRashMoveLeft = true;
            }
            else
            {
                isRashMoveLeft = false;
            }
            rashMovePhase++;
        }
        else if(rashMovePhase == 1 || rashMovePhase == 3)
        {
            anim.SetBool("isMove", true);

            Debug.Log(wallContact.getContact);

            if (!wallContact.getContact)
            {
                if (isRashMoveLeft)
                {
                    this.transform.position -= new Vector3(rashMoveSpeed, 0f, 0f);
                    Inversion(Direction.Left);
                }
                else
                {
                    this.transform.position += new Vector3(rashMoveSpeed, 0f, 0f);
                    Inversion(Direction.Right);
                }
            }
            else
            {
                anim.SetBool("isMove", false);
            }

            rashMoveDis += rashMoveSpeed;

            if (rashMoveDis >= 5.0f)
            {
                rashMoveDis = 0f;
                anim.SetBool("isMove", false);
                rashMovePhase++;
            }
        }
        else
        {
            isAttack = false;
            rashMovePhase = 0;
            rashMoveDis = 0f;
            StateChange(EnemyState.Move);
        }
    }

    //ドラゴンの状態を攻撃状態へと変更する処理
    private void AttackStateChange()
    {
        if (attackTimeCnt >= nextAttackTime)
        {
            attackTimeCnt = 0f;
            StateChange(EnemyState.Attack);
        }
        else
        {
            attackTimeCnt += Time.deltaTime;
        }
    }

    //火球生成処理
    public void CreateFireBall()
    {
        GameObject obj = Instantiate(fireBall,
            this.transform.Find("FiringPoint").transform.position,
            Quaternion.Euler(0f, 0f, 270f));
        obj.GetComponent<FireBall>().Init(disTwoPoints);
    }
}
