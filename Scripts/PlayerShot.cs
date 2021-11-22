using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{

    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        Transform();
    }

    void Transform()
    {
        //���W�̍X�V
        transform.position = _player.transform.position;

        //�X�N���[�����W���v�Z����
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //���̃I�u�W�F�N�g���猩���}�E�X�J�[�\���̕������v�Z����
        var direction = Input.mousePosition - screenPos;

        // �}�E�X�J�[�\�������݂�������̊p�x���擾����
        var angle = GetAngle(Vector3.zero, direction);

        //���̃I�u�W�F�N�g�}�E�X�J�[�\���̕���������悤�ɂ���
        //(�q�I�u�W�F�N�g�̖��@�w���ړ�����)
        var angles = transform.localEulerAngles;
        angles.z = angle;
        transform.localEulerAngles = angles;
    }
    public static float GetAngle(Vector2 from, Vector2 to)
    {   //��̓_����p�x�����߂�
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

}
