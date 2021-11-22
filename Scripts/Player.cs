using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    private float speed;
    private float jumpPower;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speed = 0.015f;
        jumpPower = 50.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.A))
        {   //ç∂à⁄ìÆ
            transform.position -= new Vector3(speed, 0, 0);
            sr.flipX = true;
            anim.SetFloat("Speed", speed);
        }
        if(Input.GetKeyUp(KeyCode.A)) anim.SetFloat("Speed", -1);
        if (Input.GetKey(KeyCode.D))
        {   //âEà⁄ìÆ
            transform.position += new Vector3(speed, 0, 0);
            sr.flipX = false;
            anim.SetFloat("Speed", speed);
        }
        if (Input.GetKeyUp(KeyCode.D)) anim.SetFloat("Speed", -1);
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(jumpPower * Vector2.up);
        }
    }
}
