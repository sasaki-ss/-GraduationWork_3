using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    private GameObject  fireBall;
    private float       disTwoPoints;

    private void Start()
    {
        player = GameObject.Find("Player");
        anim = this.GetComponent<Animator>();
        aSrc = this.GetComponent<AudioSource>();
        sr = this.GetComponent<SpriteRenderer>();
        eCollision = collisionObj[0].GetComponent<EnemyCollision>();
        wallContact = collisionObj[1].GetComponent<WallContact>();

        coolCnt = 0f;
        coolTime = 5f;
        trackingDistance = 6f;
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
        itemObj = (GameObject)Resources.Load("Item");
        disTwoPoints = 0;
    }

    private void Update()
    {
        disTwoPoints = player.transform.position.x - this.transform.position.x;

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

    public void CreateFireBall()
    {
        GameObject obj = Instantiate(fireBall,
            this.transform.position,
            Quaternion.Euler(0f, 0f, 270f));

        FireBall fb = obj.GetComponent<FireBall>();

        fb.Init(disTwoPoints);
        fb.SetScale(new Vector3(0.05f, 0.05f, 1f));
        fb.SetMoveSpeed(0.1f);

        aSrc.PlayOneShot(ac[0]);
    }
}
