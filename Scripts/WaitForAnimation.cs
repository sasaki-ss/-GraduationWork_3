using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************************************
 * ��������Q�l�ɂ��܂����� https://tsubakit1.hateblo.jp/entry/2016/02/11/021743
 ******************************************************************************/

public class WaitForAnimation : CustomYieldInstruction
{
    private Animator anim;
    private int lastStateHash = 0;
    private int layerNo = 0;

    //�R���X�g���N�^
    public WaitForAnimation(Animator _anim, int _layerNo)
    {
        Init(_anim, _layerNo, _anim.GetCurrentAnimatorStateInfo(_layerNo).fullPathHash);
    }

    //����������
    private void Init(Animator _anim, int _layerNo, int _hash)
    {
        anim = _anim;
        lastStateHash = _hash;
        layerNo = _layerNo;
    }

    //�Đ��I������𑽕����Ă�Ƃ���
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
