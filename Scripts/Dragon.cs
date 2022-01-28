using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviour

public class Dragon : Enemy
{
    private GameObject  fireBall;

    [SerializeField]
    private GameObject[]    hitCollisions;

    private FloorChecker    fc;
    private Rigidbody2D     rb2;

    private float           disTwoPoints;
    private float           nextAttackTime;
    private float           attackTimeCnt;

    [SerializeField]
    private int             rashMovePhase;
    private float           rashMoveSpeed;
    [SerializeField]
    private float           rashMoveDis;
    private bool            isRushAttack;
    private bool            isRashMoveLeft;
    private bool            isActive;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    private void Start()
    {
        atk = 25;
        hp = 7500;
        def = 5;

        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        aSrc = this.GetComponent<AudioSource>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();
        fc = collisionObj[2].GetComponent<FloorChecker>();

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

        itemObj = null;
        fireBall = (GameObject)Resources.Load("FireBall");
        hitCollisions = new GameObject[9];
        hitCollisions[0] = transform.Find("HitCollision_Body").gameObject;
        hitCollisions[1] = transform.Find("HitCollision_Head").gameObject;
        hitCollisions[2] = transform.Find("HitCollision_Neck").gameObject;
        hitCollisions[3] = transform.Find("HitCollision_Wing").gameObject;
        hitCollisions[4] = transform.Find("HitCollision_Tail").gameObject;
        hitCollisions[5] = transform.Find("HitCollision_LegL/Up").gameObject;
        hitCollisions[6] = transform.Find("HitCollision_LegR/Up").gameObject;
        hitCollisions[7] = transform.Find("HitCollision_Leg2L/Up").gameObject;
        hitCollisions[8] = transform.Find("HitCollision_Leg2R/Up").gameObject;

        foreach(GameObject obj in hitCollisions)
        {
            obj.SetActive(false);
        }

        SetCollisionIsTrigger(false);

        rb2 = GetComponent<Rigidbody2D>();

        collisionObj[3].SetActive(false);

        disTwoPoints = 0;
        nextAttackTime = 5f;
        attackTimeCnt = 0f;
        rashMovePhase = 0;
        rashMoveSpeed = 0.1f;
        rashMoveDis = 0;
        isRushAttack = false;
        isRashMoveLeft = false;
        isActive = false;
    }

    private void FixedUpdate()
    {
        if (fc.isFloor)
        {
            rb2.gravityScale = 0;
        }
        else
        {
            rb2.gravityScale = 1f;
        }
    }

    private void Update()
    {
        if (!isActive) return;

        disTwoPoints = player.transform.position.x - this.transform.position.x;

        if (!isAttack) AttackStateChange();

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
                    else if(beforeState == EnemyState.Move)
                    {
                        StartCoroutine(Wait(waitTime / 10));
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

        if (!aSrc.isPlaying)
        {
            aSrc.clip = ac[2];
            aSrc.Play();
        }

        if (!wallContact.getContact)
        {
            //disTwoPointsが-なら左へ、+なら右へ移動、0の場合移動しない
            if (disTwoPoints < -0.1)
            {
                this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
                Inversion(Direction.Left);
            }
            else if (disTwoPoints > 0.1)
            {
                this.transform.position += new Vector3(moveSpeed, 0f, 0f);
                Inversion(Direction.Right);
            }
            else
            {
                anim.SetBool("isMove", false);
                if (aSrc.isPlaying) aSrc.Stop();
            }
        }
        else
        {
            if(dir == Direction.Left)
            {
                Inversion(Direction.Right);
            }
            else if(dir == Direction.Right)
            {
                Inversion(Direction.Left);
            }

            StateChange(EnemyState.Wait);
            anim.SetBool("isMove", false);
            if (aSrc.isPlaying) aSrc.Stop();
            return;
        }
    }

    private void AttackProc()
    {
        //
        if (eCollision.isInvasion)
        {
            if (wallContact.getContact)
            {
                StartCoroutine(Attack());
                return;
            }

            collisionObj[3].SetActive(true);
            SetCollisionIsTrigger(true);
            if (aSrc.isPlaying) aSrc.Stop();
            isRushAttack = true;
            return;
        }
        //攻撃判定外の場合ブレス攻撃
        else
        {
            if (dir == Direction.Left)
            {
                if (transform.position.x - 0.5 > player.transform.position.x)
                {
                    StartCoroutine(Attack());
                    return;
                }
            }
            else if (dir == Direction.Right)
            {
                if (transform.position.x - 0.5 < player.transform.position.x)
                {
                    StartCoroutine(Attack());
                    return;
                }
            }

            isAttack = false;
            StateChange(EnemyState.Move);
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
            if (!aSrc.isPlaying)
            {
                aSrc.clip = ac[1];
                aSrc.Play();
            }

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
                rashMovePhase = 4;
                return;
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
            isRushAttack = false;
            rashMovePhase = 0;
            rashMoveDis = 0f;
            if (aSrc.isPlaying) aSrc.Stop();
            SetCollisionIsTrigger(false);
            collisionObj[3].SetActive(false);
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

    private void SetCollisionIsTrigger(bool _val)
    {
        foreach(GameObject obj in hitCollisions)
        {
            BoxCollider2D bx2 = obj.GetComponent<BoxCollider2D>();
            bx2.isTrigger = _val;
        }
    }

    //火球生成処理
    public void CreateFireBall()
    {
        GameObject obj = Instantiate(fireBall,
            this.transform.Find("FiringPoint").transform.position,
            Quaternion.Euler(0f, 0f, 270f));
        obj.GetComponent<FireBall>().Init(disTwoPoints);

        aSrc.PlayOneShot(ac[0]);
    }

    public void SetCollision()
    {
        foreach(GameObject obj in hitCollisions)
        {
            obj.SetActive(true);
        }
    }
}
