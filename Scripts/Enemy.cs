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
    protected int               hp;             //�̗�
    protected float             atk;            //�U����
    protected float             def;            //�h���
    protected float             coolTime;       //�N�[���^�C��
    protected float             coolCnt;        //���݂̃N�[���J�E���g
    protected bool              isCoolDown;     //�N�[���_�E���t���O
    [SerializeField]
    protected GameObject        collisionObj;   //�����蔻��I�u�W�F�N�g
    protected Animator          anim;           //�A�j���[�^�[
    protected EnemyCollision    eCollision;     //�����蔻��p�̏���

    protected void CoolTime()
    {
        //�N�[���_�E���t���O���I���̏ꍇ
        if (isCoolDown)
        {
            //�N�[���_�E���̏���
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
