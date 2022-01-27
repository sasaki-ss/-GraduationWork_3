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

//敵の移動パターン
public enum EnemyMovePattern
{
    Normal,
    Event
}

public class Enemy : MonoBehaviour
{
    public enum Direction
    {
        Left,   //左
        Right   //右
    }

    protected int               hp;                 //体力
    protected int               atk;                //攻撃力
    protected int               def;                //防御力

    protected float             trackingDistance;   //追跡距離
    protected float             moveSpeed;          //移動速度

    protected float             waitTime;           //待機時間
    protected float             coolTime;           //クールタイム
    protected float             coolCnt;            //現在のクールカウント

    protected bool              isCoolDown;         //クールダウンフラグ
    protected bool              isWait;             //待機フラグ
    protected bool              isAttack;           //攻撃フラグ
    protected bool              isTracking;         //追跡フラグ

    [SerializeField]
    protected Direction         dir;                //方向

    protected EnemyState        beforeState =
        EnemyState.Wait;                            //一つ前の状態
    [SerializeField]
    protected EnemyState        state = 
        EnemyState.Wait;                            //状態
    [SerializeField]
    protected EnemyMovePattern  eMovePattern = 
        EnemyMovePattern.Normal;                    //敵の移動パターン

    [SerializeField]
    protected GameObject[]      collisionObj;       //当たり判定オブジェクト
    protected GameObject        player;             //プレイヤーオブジェクト
    protected Animator          anim;               //アニメーター
    protected AudioSource       aSrc;               //オーディオソース
    [SerializeField]
    protected AudioClip[]       ac;                 //オーディオCLIP
    protected SpriteRenderer    sr;                 //スプライトレンダラー
    protected EnemyCollision    eCollision;         //当たり判定用の処理
    protected WallContact       wallContact;        //壁判定用の処理
    [SerializeField]
    protected GameObject        itemObj;            //アイテムオブジェクト

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
                Inversion(Direction.Left);

            if (player.transform.position.x > this.transform.position.x + 0.02)
                Inversion(Direction.Right);
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

    //反転処理
    protected void Inversion(Direction _dir)
    {
        //反転で指定した方向が現在の方向と同じ場合処理を行わない
        if (dir == _dir) return;

        dir = _dir;
        float scale = Mathf.Abs(this.transform.localScale.x);

        //左方向の際
        if(dir == Direction.Left)
        {
            this.transform.localScale = new Vector3(scale, scale, 0f);
        }

        //右方向の際
        if(dir == Direction.Right)
        {
            this.transform.localScale = new Vector3(-scale, scale, 0f);
        }
    }

    protected void MoveProc()
    {
        if (dir == Direction.Left)
        {
            Inversion(Direction.Left);

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
            Inversion(Direction.Right);

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

    public void CreateItem()
    {
        if(itemObj != null)
        {
            GameObject obj = Instantiate(itemObj, this.transform.position, Quaternion.identity);
        }
    }

    //ダメージ処理
    public void Damage(int _bulletPow)
    {
        hp -= (_bulletPow - def);

        if(hp <= 0)
        {
            //aSrc.PlayOneShot(ac[ac.Length - 1]);
            CreateItem();
            Destroy(this.gameObject);
        }
    }

    public void SetEnemyMovePattern(EnemyMovePattern _eMovePattern)
    {
        eMovePattern = _eMovePattern;
        StateChange(EnemyState.Move);
    }

    public int GetAtk()
    {
        return atk;
    }
}
