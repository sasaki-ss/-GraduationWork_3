using UnityEngine;

public class Player : MonoBehaviour
{
    //プレイヤーのコンポーネント
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    //ステータス
    private float speed;        //移動速度
    private float jumpPower;    //ジャンプの高さ
    private int jumpCount;      //ジャンプ回数のカウント

    //弾関連
    private GameObject MagicArray;  //魔法陣
    private GameObject shot_0;      //弾
    private int shot_speed;         //弾の速度
    private int cooltime;           //弾発射のクールタイム
    

    //定数
    private const int MaxJumpCount = 2; //最大ジャンプ回数
    private const int CoolTime0 = 5;    //shot_0のクールタイム

    //フレームカウント
    private int count;

    //壁判定
    GameObject[] _wallContact;
    WallContact[] scr_WallContact;

    //床判定
    GameObject _floorContact;
    FloorContact scr_FloorContact;

    void Start()
    {
        //プレイヤーのコンポーネント
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //ステータス
        speed = 0.05f;
        jumpPower = 280.0f;
        jumpCount = 0;

        //弾関連
        MagicArray = transform.Find("PlayerShot/MagicArray").gameObject;
        shot_0 = (GameObject)Resources.Load("shot_0");
        shot_speed = 20;
        cooltime = 0;

        //フレームカウント
        count = 0;

        //壁判定
        _wallContact = new GameObject[2];
        scr_WallContact = new WallContact[2];
        _wallContact[0] = transform.Find("Contact_L").gameObject;
        _wallContact[1] = transform.Find("Contact_R").gameObject;
        scr_WallContact[0] = _wallContact[0].GetComponent<WallContact>();
        scr_WallContact[1] = _wallContact[1].GetComponent<WallContact>();

        //床判定
        _floorContact = transform.Find("Contact_Down").gameObject;
        scr_FloorContact = _floorContact.GetComponent<FloorContact>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
        Shot();
        count++;
        cooltime++;
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
        if (Input.GetKey(KeyCode.Space) && jumpCount < MaxJumpCount && 30 < count)
        {
            rb.velocity = Vector2.zero;             //速度リセット
            rb.AddForce(jumpPower * Vector2.up);    //力を加える
            if(jumpCount == 0)jumpCount = 1;        //カウント加算
            if(jumpCount == 1)jumpCount = 2;        //カウント加算
            count = 0;                              //ジャンプのクールタイムリセット
        }

        if (scr_FloorContact.getFloorContact)
        {
            jumpCount = 0;    //ジャンプ回数リセット
        }
    }

    void Shot()
    {
        if (Input.GetMouseButton(0))        //左クリック
        {
            if (CoolTime0 < cooltime)
            {
                //生成
                GameObject shot = Instantiate(shot_0, MagicArray.transform.position, Quaternion.identity);

                // クリックした座標の取得（スクリーン座標からワールド座標に変換）
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // 向きの生成（Z成分の除去と正規化）
                Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

                // 弾に速度を与える
                shot.GetComponent<Rigidbody2D>().velocity = shotForward * shot_speed;

                cooltime = 0;   //クールタイムリセット
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
