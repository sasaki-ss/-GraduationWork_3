using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobold : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("isAttack", true);
            anim.SetBool("isMove", true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            anim.SetBool("isAttack", false);
        }
    }
}
