using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackProc : MonoBehaviour
{
    [SerializeField]
    GameObject parentObject;    //親オブジェクト

    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーと当たったとき、当たりフラグとダメージを受け渡す
        if(other.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            player.SetHitFlg = true;
            player.SetDamage = parentObject.GetComponent<Enemy>().GetAtk();
        }
    }
}
