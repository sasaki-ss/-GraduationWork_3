using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float   moveSpeed;  //�ړ����x
    private Vector3 velocity;   //���x
    [SerializeField]
    private Vector3 scale;

    //����������
    private void Start()
    {
        moveSpeed = 0.2f;
    }

    //�X�V����
    private void Update()
    {
        //���W�̍X�V
        this.transform.position += velocity * moveSpeed;

        //��ʊO�ɏo���ۂ��̃I�u�W�F�N�g������
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    //����������
    public void Init(float val)
    {
        scale = new Vector3(0.1f, 0.1f, 1f);

        //�v���C���[�Ɖ΋��Ԃ̒P�ʃx�N�g�����擾
        GameObject player = GameObject.Find("Player");
        Vector3 twoPointVec = player.transform.position - this.transform.position;
        velocity = twoPointVec.normalized;

        //�v���C���[���h���S�����E���ɂ���۔��]
        if(val > 0)
        {
            transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
        }
    }

    public void SetScale(Vector3 _scale)
    {
        scale = _scale;
        transform.localScale = scale;
    }

    //�����蔻�菈��
    private void OnTriggerEnter2D(Collider2D other)
    {
        //���������I�u�W�F�N�g���v���C���[�̍�
        if(other.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.SetHitFlg = true;
            player.SetDamage = 50;

            Destroy(this.gameObject);
        }
    }
}
