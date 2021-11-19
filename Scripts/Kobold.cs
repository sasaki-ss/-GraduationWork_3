using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : MonoBehaviour
{
    private bool isCoolDown;    //クールダウンフラグ

    private Animator anim;      //アニメーター

    private void Start()
    {
        isCoolDown = false;
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !isCoolDown)
        {
            StartCoroutine(Attack());
        }
    }

    //攻撃処理
    private IEnumerator Attack()
    {
        anim.SetBool("isAttack", true);
        isCoolDown = true;

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        anim.SetBool("isAttack", false);
    }
}
