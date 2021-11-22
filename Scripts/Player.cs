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

    private const int MaxJumpCount = 2;
    private int jumpCount;

    private int count;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speed = 0.1f;
        jumpPower = 280.0f;
        jumpCount = 0;

        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
        count++;
        if (60 < count) jumpCount = 0;
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.A))
        {   //¶ˆÚ“®
            transform.position -= new Vector3(speed, 0, 0);
            sr.flipX = true;
            anim.SetFloat("Speed", speed);
        }
        if(Input.GetKeyUp(KeyCode.A)) anim.SetFloat("Speed", -1);

        if (Input.GetKey(KeyCode.D))
        {   //‰EˆÚ“®
            transform.position += new Vector3(speed, 0, 0);
            sr.flipX = false;
            anim.SetFloat("Speed", speed);
        }
        if (Input.GetKeyUp(KeyCode.D)) anim.SetFloat("Speed", -1);
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && jumpCount <MaxJumpCount && 30 < count)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(jumpPower * Vector2.up);
            jumpCount++;
            count = 0;
        }
    }
}
