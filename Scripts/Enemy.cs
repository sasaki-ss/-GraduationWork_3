using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wait,
    Move,
    Attack,
    Tracking
}

public class Enemy : MonoBehaviour
{
    public enum Direction
    {
        Left,   //左
        Right   //右
    }

    protected int               hp;                 //体力
    protected float             atk;                //攻撃力
    protected float             def;                //防御力

    protected float             trackingDistance;   //追跡距離
    protected float             moveSpeed;          //移動速度

    protected float             waitTime;           //待機時間
    protected float             coolTime;           //クールタイム
    protected float             coolCnt;            //現在のクールカウント

    protected bool              isCoolDown;         //クールダウンフラグ
    protected bool              isWait;             //待機フラグ
    protected bool              isAttack;           //攻撃フラグ
    protected bool              isTracking;         //追跡フラグ

    protected Vector2[]         colOffset;          //当たり判定の偏り値
    protected Direction         dir;                //方向

    protected EnemyState        beforeState;        //一つ前の状態
    [SerializeField]
    protected EnemyState        state;              //状態

    [SerializeField]
    protected GameObject[]      collisionObj;       //当たり判定オブジェクト
    protected GameObject        player;             //プレイヤーオブジェクト
    protected Animator          anim;               //アニメーター
    protected BoxCollider2D[]   bc2;                //ボックスコライダー2d
    protected SpriteRenderer    sr;                 //スプライトレンダラー
    protected EnemyCollision    eCollision;         //当たり判定用の処理
    protected WallContact       wallContact;        //壁判定用の処理

    //初期化処理
    protected void Init()
    {
        bc2 = new BoxCollider2D[2];
        colOffset = new Vector2[2];
    }

    //状態変化処理
    protected void StateChange(EnemyState _state)
    {
        beforeState = state;
        state = _state;
    }

    //クールダウン処理
    protected void CoolTime()
    {
        //クールダウンフラグがオンの場合
        if (isCoolDown)
        {
            //クールダウンの処理
            if (coolCnt >= coolTime)
            {
                isCoolDown = false;
                coolCnt = 0f;
            }
            else
            {
                coolCnt += Time.deltaTime;
            }
        }
    }

    //追跡判定処理
    protected void TrackingJudgment()
    {
        if (isTracking)
        {
            //状態がWaitまたはAttackのとき
            if(state == EnemyState.Wait || state == EnemyState.Attack)
            {
                isTracking = false;
                return;
            }

            if (player.transform.position.x < this.transform.position.x - 0.02)
                dir = Direction.Left;

            if (player.transform.position.x > this.transform.position.x + 0.02)
                dir = Direction.Right;
        }

        if (dir == Direction.Left)
        {
            if (player.transform.position.x >=
                this.transform.position.x - trackingDistance &&
                player.transform.position.x <= this.transform.position.x)
            {
                StateChange(EnemyState.Tracking);
                isTracking = true;
                return;
            }
        }

        if (dir == Direction.Right)
        {
            if (player.transform.position.x <=
                this.transform.position.x + trackingDistance &&
                player.transform.position.x >= this.transform.position.x)
            {
                StateChange(EnemyState.Tracking);
                isTracking = true;
                return;
            }
        }

        if (state == EnemyState.Tracking)
        {
            StateChange(EnemyState.Wait);
            anim.SetBool("isMove", false);
            isTracking = false;
            isWait = false;
        }
    }

    //待機処理
    protected IEnumerator Wait(float _waitTime)
    {
        anim.SetBool("isMove", false);

        //ステートがAttack,Trackingの時コルーチンを終了させる
        if (state == EnemyState.Attack ||
            state == EnemyState.Tracking) yield break;

        yield return new WaitForSeconds(_waitTime);

        StateChange(EnemyState.Move);
    }

    //攻撃処理
    protected IEnumerator Attack()
    {
        anim.SetBool("isAttack", true);

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        anim.SetBool("isAttack", false);
        StateChange(EnemyState.Wait);
        isWait = false;
        isAttack = false;
        isCoolDown = true;
    }

    //追跡処理
    protected void Tracking()
    {
        if (!eCollision.isInvasion && !isAttack)
        {
            MoveProc();
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    protected void MoveProc()
    {
        if (dir == Direction.Left)
        {
            sr.flipX = false;
            bc2[0].offset = new Vector2(-colOffset[0].x, colOffset[0].y);
            bc2[1].offset = new Vector2(-colOffset[1].x, colOffset[1].y);

            if (!wallContact.getContact)
            {
                anim.SetBool("isMove", true);
                this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
            }
            else
            {
                anim.SetBool("isMove", false);
            }
        }

        if(dir == Direction.Right)
        {
            sr.flipX = true;
            bc2[0].offset = new Vector2(colOffset[0].x, colOffset[0].y);
            bc2[1].offset = new Vector2(colOffset[1].x, colOffset[1].y);

            if (!wallContact.getContact)
            {
                anim.SetBool("isMove", true);
                this.transform.position += new Vector3(moveSpeed, 0f, 0f);
            }
            else
            {
                anim.SetBool("isMove", false);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("当たりました");
        }
    }
}
