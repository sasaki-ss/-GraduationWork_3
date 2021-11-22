using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wait,
    Move,
    Attack
}

public class Enemy : MonoBehaviour
{
    protected int               hp;             //体力
    protected float             atk;            //攻撃力
    protected float             def;            //防御力
    protected float             coolTime;       //クールタイム
    protected float             coolCnt;        //現在のクールカウント
    protected bool              isCoolDown;     //クールダウンフラグ
    [SerializeField]
    protected GameObject        collisionObj;   //当たり判定オブジェクト
    protected Animator          anim;           //アニメーター
    protected EnemyCollision    eCollision;     //当たり判定用の処理

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
}
