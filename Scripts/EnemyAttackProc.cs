using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackProc : MonoBehaviour
{
    GameObject parentObject;

    private void Start()
    {
        parentObject = this.transform.root.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            player.SetHitFlg = true;
            player.SetDamage = parentObject.GetComponent<Enemy>().GetAtk();
        }
    }
}
