using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackProc : MonoBehaviour
{
    [SerializeField]
    GameObject parentObject;    //�e�I�u�W�F�N�g

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�v���C���[�Ɠ��������Ƃ��A������t���O�ƃ_���[�W���󂯓n��
        if(other.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            player.SetHitFlg = true;
            player.SetDamage = parentObject.GetComponent<Enemy>().GetAtk();
        }
    }
}
