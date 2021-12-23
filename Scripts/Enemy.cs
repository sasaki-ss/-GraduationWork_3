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

//�G�̈ړ��p�^�[��
public enum EnemyMovePattern
{
    Normal,
    Event
}

public class Enemy : MonoBehaviour
{
    public enum Direction
    {
        Left,   //��
        Right   //�E
    }

    protected int               hp;                 //�̗�
    protected int               atk;                //�U����
    protected int               def;                //�h���

    protected float             trackingDistance;   //�ǐՋ���
    protected float             moveSpeed;          //�ړ����x

    protected float             waitTime;           //�ҋ@����
    protected float             coolTime;           //�N�[���^�C��
    protected float             coolCnt;            //���݂̃N�[���J�E���g

    protected bool              isCoolDown;         //�N�[���_�E���t���O
    protected bool              isWait;             //�ҋ@�t���O
    protected bool              isAttack;           //�U���t���O
    protected bool              isTracking;         //�ǐՃt���O

    protected Vector2[]         colOffset;          //�����蔻��̕΂�l
    protected Direction         dir;                //����

    protected EnemyState        beforeState;        //��O�̏��
    [SerializeField]
    protected EnemyState        state;              //���
    [SerializeField]
    protected EnemyMovePattern  eMovePattern = 
        EnemyMovePattern.Normal;                    //�G�̈ړ��p�^�[��

    [SerializeField]
    protected GameObject[]      collisionObj;       //�����蔻��I�u�W�F�N�g
    protected GameObject        player;             //�v���C���[�I�u�W�F�N�g
    protected Animator          anim;               //�A�j���[�^�[
    protected BoxCollider2D[]   bc2;                //�{�b�N�X�R���C�_�[2d
    protected SpriteRenderer    sr;                 //�X�v���C�g�����_���[
    protected EnemyCollision    eCollision;         //�����蔻��p�̏���
    protected WallContact       wallContact;        //�ǔ���p�̏���

    //����������
    protected void Init()
    {
        bc2 = new BoxCollider2D[2];
        colOffset = new Vector2[2];
    }

    //��ԕω�����
    protected void StateChange(EnemyState _state)
    {
        beforeState = state;
        state = _state;
    }

    //�N�[���_�E������
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

    //�ǐՔ��菈��
    protected void TrackingJudgment()
    {
        if (isTracking)
        {
            //��Ԃ�Wait�܂���Attack�̂Ƃ�
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

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        anim.SetBool("isAttack", false);
        StateChange(EnemyState.Wait);
        isWait = false;
        isAttack = false;
        isCoolDown = true;
    }

    //�ǐՏ���
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

    //���]����
    protected void Inversion(Direction _dir)
    {
        //���]�Ŏw�肵�����������݂̕����Ɠ����ꍇ�������s��Ȃ�
        if (dir == _dir) return;

        dir = _dir;
        float scale = Mathf.Abs(this.transform.localScale.x);

        //�������̍�
        if(dir == Direction.Left)
        {
            this.transform.localScale = new Vector3(scale, scale, 0f);
        }

        //�E�����̍�
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

    //�_���[�W����
    public void Damage(int _bulletPow)
    {
        hp -= (_bulletPow - def);

        Debug.Log("�_���[�W!! now HP = " + hp);

        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetEnemyMovePattern(EnemyMovePattern _eMovePattern)
    {
        eMovePattern = _eMovePattern;
    }

    public int GetAtk()
    {
        return atk;
    }
}
