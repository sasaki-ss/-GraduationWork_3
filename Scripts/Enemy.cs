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
        Left,   //��
        Right   //�E
    }

    public enum MoveState
    {
        MoveOne,
        MoveTwo,
    }

    protected int               hp;                 //�̗�

    protected float             atk;                //�U����
    protected float             def;                //�h���
    protected float             trackingDistance;   //�ǐՋ���
    protected float             moveSpeed;          //�ړ����x
    protected float             waitTime;           //�ҋ@����
    protected float             coolTime;           //�N�[���^�C��
    protected float             coolCnt;            //���݂̃N�[���J�E���g

    protected bool              isCoolDown;         //�N�[���_�E���t���O
    protected bool              isWait;             //�ҋ@�t���O
    protected bool              isAttack;           //�U���t���O
    protected bool              isTracking;         //�ǐՃt���O

    protected Vector2           colOffset;          //�����蔻��̕΂�l

    protected Direction         dir;                //����

    protected EnemyState        beforeState;        //��O�̏��
    [SerializeField]
    protected EnemyState        state;              //���

    [SerializeField]
    protected GameObject        collisionObj;       //�����蔻��I�u�W�F�N�g
    protected GameObject        player;             //�v���C���[�I�u�W�F�N�g

    protected Animator          anim;               //�A�j���[�^�[

    protected BoxCollider2D     bc2;                //�{�b�N�X�R���C�_�[2d

    protected SpriteRenderer    sr;                 //�X�v���C�g�����_���[

    protected EnemyCollision    eCollision;         //�����蔻��p�̏���

    protected void StateChange(EnemyState _state)
    {
        beforeState = state;
        state = _state;
    }

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

    //�ҋ@����
    protected IEnumerator Wait(float _waitTime)
    {
        anim.SetBool("isMove", false);

        //�X�e�[�g��Attack,Tracking�̎��R���[�`�����I��������
        if (state == EnemyState.Attack ||
            state == EnemyState.Tracking) yield break;

        yield return new WaitForSeconds(_waitTime);

        StateChange(EnemyState.Move);
    }

    //�U������
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

    //�ړ�����(��ǐՎ�)
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

    //�ǐՏ���
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
