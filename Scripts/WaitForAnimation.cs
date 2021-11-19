using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************************************
 * こちらを参考にしました→ https://tsubakit1.hateblo.jp/entry/2016/02/11/021743
 ******************************************************************************/

public class WaitForAnimation : CustomYieldInstruction
{
    private Animator anim;
    private int lastStateHash = 0;
    private int layerNo = 0;

    //コンストラクタ
    public WaitForAnimation(Animator _anim, int _layerNo)
    {
        Init(_anim, _layerNo, _anim.GetCurrentAnimatorStateInfo(_layerNo).fullPathHash);
    }

    //初期化処理
    private void Init(Animator _anim, int _layerNo, int _hash)
    {
        anim = _anim;
        lastStateHash = _hash;
        layerNo = _layerNo;
    }

    //再生終了判定を多分してるところ
    public override bool keepWaiting
    {
        get
        {
            var currentAnimState = anim.GetCurrentAnimatorStateInfo(layerNo);
            return currentAnimState.fullPathHash == lastStateHash &&
                (currentAnimState.normalizedTime < 1);
        }
    }
}
