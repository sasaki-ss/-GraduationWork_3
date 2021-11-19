using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    private float speed;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speed = 0.03f;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.A))
        {   //ç∂à⁄ìÆ
            transform.position -= new Vector3(speed, 0, 0);
            sr.flipX = true;
        }

        if (Input.GetKey(KeyCode.D))
        {   //âEà⁄ìÆ
            transform.position += new Vector3(speed, 0, 0);
            sr.flipX = false;
        }
    }
}
