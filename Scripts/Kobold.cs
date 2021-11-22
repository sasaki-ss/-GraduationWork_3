using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : Enemy
{
    private void Start()
    {
        coolCnt = 0f;
        coolTime = 3f;
        isCoolDown = false;
        anim = this.GetComponent<Animator>();
        eCollision = collisionObj.GetComponent<EnemyCollision>();
    }

    private void Update()
    {
        CoolTime();

        if (!isCoolDown && eCollision.isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    //çUåÇèàóù
    private IEnumerator Attack()
    {
        anim.SetBool("isAttack", true);
        isCoolDown = true;

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        anim.SetBool("isAttack", false);
    }

    private void Move()
    {
        anim.SetBool("isMove", true);
    }
}
