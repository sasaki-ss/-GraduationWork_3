using UnityEngine;

public class Player : MonoBehaviour
{
    //プレイヤーのコンポーネント
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    //ステータス
    private bool isActive;      //アクティブ状態
    private float speed;        //移動速度
    private float jumpPower;    //ジャンプの高さ
    private int jumpCount;      //ジャンプ回数のカウント

    //弾関連
    private GameObject MagicArray;  //魔法陣
    private GameObject[] ShotObject;//弾
    private int shotSelect;         //現在選択されてるショット
    private int cooltime;           //弾発射のクールタイム

    //マウスクリック
    private bool clickFlg;          //単発ショット用

    //定数
    private const int MaxJumpCount = 2;            //最大ジャンプ回数

    //弾関連の定数
    private const int ShotType = 2;                //ショットの種類
    private readonly int[] CoolTime = { 5,15 };    //ショットのクールタイム配列
    private readonly int[] ShotPower = { 5,25 };　 //ショットの攻撃力配列
    private readonly int[] ShotSpeed = { 20,10 };  //弾の速度配列

    //フレームカウント
    private int count;

    //壁判定
    GameObject[] _wallContact;
    WallContact[] scr_WallContact;

    //床判定
    GameObject _floorContact;
    FloorContact scr_FloorContact;

    public int GetShotPower{
        get { return ShotPower[shotSelect]; }
    }

    void Start()
    {
        //プレイヤーのコンポーネント
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //ステータス
        isActive = true;
        speed = 0.05f;
        jumpPower = 280.0f;
        jumpCount = 0;

        //弾関連
        MagicArray = transform.Find("PlayerShot/MagicArray").gameObject;
        ShotObject = new GameObject[ShotType];
        ShotObject[0] = (GameObject)Resources.Load("shot_0");
        ShotObject[1] = (GameObject)Resources.Load("shot_0");
        shotSelect = 0;
        cooltime = 0;

        //マウスクリック
        clickFlg = false;

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
            shotSelect = 1;

            if (CoolTime[shotSelect] < cooltime)
            {
                switch (shotSelect)
                {
                    case 0:
                        Shot_0();
                        break;
                    case 1:
                        if(!clickFlg) Shot_1();
                        clickFlg = true;
                        break;
                }

                cooltime = 0;   //クールタイムリセット
            }
        }
        if (Input.GetMouseButtonUp(0)) clickFlg = false;
    }

    void Shot_0()
    {   //通常のショット

        //生成
        GameObject shot = Instantiate(ShotObject[shotSelect], MagicArray.transform.position, Quaternion.identity);

        // クリックした座標の取得（スクリーン座標からワールド座標に変換）
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 向きの生成（Z成分の除去と正規化）
        Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

        // 弾に速度を与える
        shot.GetComponent<Rigidbody2D>().velocity = shotForward * ShotSpeed[shotSelect];
    }

    void Shot_1()
    {   //単発

        Shot_0();
    }
}
