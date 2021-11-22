using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //プレイヤーのコンポーネント
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    //ステータス
    private float speed;
    private float jumpPower;
    private int jumpCount;

    //定数
    private const int MaxJumpCount = 2;

    //フレームカウント
    private int count;

    //壁判定
    GameObject[] _wallContact;
    WallContact[] scr_WallContact;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speed = 0.1f;
        jumpPower = 280.0f;
        jumpCount = 0;

        count = 0;

        _wallContact = new GameObject[2];
        scr_WallContact = new WallContact[2];
        _wallContact[0] = transform.Find("Contact_L").gameObject;
        _wallContact[1] = transform.Find("Contact_R").gameObject;
        scr_WallContact[0] = _wallContact[0].GetComponent<WallContact>();
        scr_WallContact[1] = _wallContact[1].GetComponent<WallContact>();
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
        if (Input.GetKey(KeyCode.A) && !scr_WallContact[0].getContact)
        {   //左移動
            transform.position -= new Vector3(speed, 0, 0);
            sr.flipX = true;
            anim.SetFloat("Speed", speed);
        }
        else if (Input.GetKey(KeyCode.D) && !scr_WallContact[1].getContact)
        {   //右移動
            transform.position += new Vector3(speed, 0, 0);
            sr.flipX = false;
            anim.SetFloat("Speed", speed);
        }
        else anim.SetFloat("Speed", -1);
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
