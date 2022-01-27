using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    private void Start()
    {
        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        aSrc = this.GetComponent<AudioSource>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        collisionObj[2].SetActive(false);

        atk = 10;
        hp = 30;
        def = 5;

        coolCnt = 0f;
        coolTime = 1f;
        trackingDistance = 10f;
        moveSpeed = 0.02f;
        waitTime = 5f;
        isCoolDown = false;
        isWait = false;
        isAttack = false;
        isTracking = false;
        dir = Direction.Left;

        //itemObj = (GameObject)Resources.Load("Item");
        itemObj = null;
    }

    private void Update()
    {
        CoolTime();

        if (eMovePattern != EnemyMovePattern.Event) TrackingJudgment();

        if (!isAttack) collisionObj[2].SetActive(false);

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
        anim.SetBool("isMove", true);

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

    public void PlayAttckSound()
    {
        aSrc.PlayOneShot(ac[0]);
    }
}
