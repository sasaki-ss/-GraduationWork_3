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

    public enum MoveState
    {
        MoveOne,
        MoveTwo,
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

    protected Vector2           colOffset;          //当たり判定の偏り値

    protected Direction         dir;                //方向

    protected EnemyState        beforeState;        //一つ前の状態
    [SerializeField]
    protected EnemyState        state;              //状態

    [SerializeField]
    protected GameObject        collisionObj;       //当たり判定オブジェクト
    protected GameObject        player;             //プレイヤーオブジェクト

    protected Animator          anim;               //アニメーター

    protected BoxCollider2D     bc2;                //ボックスコライダー2d

    protected SpriteRenderer    sr;                 //スプライトレンダラー

    protected EnemyCollision    eCollision;         //当たり判定用の処理

    protected void StateChange(EnemyState _state)
    {
        beforeState = state;
        state = _state;
    }

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

    protected void TrackingJudgment()
    {
        if (isTracking &&
            (state == EnemyState.Wait || state == EnemyState.Attack)) return;

        if (isTracking)
        {
            if (player.transform.position.x < this.transform.position.x)
                dir = Direction.Left;

            if (player.transform.position.x > this.transform.position.x)
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
            isTracking = false;
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
        isCoolDown = true;

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        anim.SetBool("isAttack", false);
        StateChange(EnemyState.Wait);
        isWait = false;
        isAttack = false;
    }

    //移動処理(非追跡時)
    protected void Move()
    {
        anim.SetBool("isMove", true);

        if(dir == Direction.Left)
        {
            sr.flipX = false;
            bc2.offset = new Vector2(-colOffset.x, colOffset.y);
            //this.transform.position -= new Vector3(0.03f, 0f, 0f);
        }

        if(dir == Direction.Right)
        {
            sr.flipX = true;
            bc2.offset = new Vector2(colOffset.x, colOffset.y);
        }

    }

    //追跡処理
    protected void Tracking()
    {
        if (!eCollision.isAttack)
        {
            anim.SetBool("isMove", true);
            if (dir == Direction.Left)
            {
                sr.flipX = false;
                bc2.offset = new Vector2(-colOffset.x, colOffset.y);
                this.transform.position -= new Vector3(moveSpeed, 0f, 0f);
            }

            if (dir == Direction.Right)
            {
                sr.flipX = true;
                bc2.offset = new Vector2(colOffset.x, colOffset.y);
                this.transform.position += new Vector3(moveSpeed, 0f, 0f);
            }
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
}
