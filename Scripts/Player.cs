using UnityEngine;

public class Player : MonoBehaviour
{
    //�v���C���[�̃R���|�[�l���g
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    //�J����(�ړ������p)
    GameObject mainCam;
    Camera camera;
    FollowCamera followCam;
    Vector3 topLeft;
    Vector3 bottomRight;

    //�X�e�[�^�X
    private bool isActive;      //�A�N�e�B�u���
    private int hp;             //�̗�
    private float speed;        //�ړ����x
    private float jumpPower;    //�W�����v�̍���
    private int jumpCount;      //�W�����v�񐔂̃J�E���g

    //�q�b�g�֘A
    private bool hitFlg;        //�G�̍U���ɓ��������t���O
    private int damage;         //�G����̃_���[�W (�U����)
    private int hitCount;       //���G���ԗp�J�E���g
    private const int INVINCIBLE = 60;  //���G����

    //�e�֘A
    private GameObject MagicArray;  //���@�w
    private GameObject[] ShotObject;//�e
    private int shotSelect;         //���ݑI������Ă�V���b�g
    private int cooltime;           //�e���˂̃N�[���^�C��
    private int bulletCnt;          //�e���˂̃J�E���g
    private bool bulletCntFlg;      //�J�E���g�J�n�t���O
    private int bulletNumCnt;       //�e�̔��ː��J�E���g

    //�}�E�X�N���b�N
    private bool clickFlg;          //�P���V���b�g�p

    //�萔
    private const int MaxJumpCount = 2;            //�ő�W�����v��

    //�e�֘A�̒萔
    private const int ShotType = 4;                //�V���b�g�̎��
    private readonly int[] CoolTime = { 10,15,5,1 };   //�V���b�g�̃N�[���^�C���z��
    private readonly int[] ShotPower = { 10,45,10,20 };�@ //�V���b�g�̍U���͔z��
    private readonly int[] ShotSpeed = { 20,10,20,15 };  //�e�̑��x�z��

    //�t���[���J�E���g
    private int count;

    //�ǔ���
    GameObject[] _wallContact;
    WallContact[] scr_WallContact;

    //������
    GameObject _floorContact;
    FloorContact scr_FloorContact;

    public int getHP
    {
        get { return hp; }
    }

    public int getShotSelect
    {
        get { return shotSelect; }
    }

    public bool SetHitFlg
    {
        set { hitFlg = value; }
    }

    public int SetDamage
    {
        set { damage = value; }
    }

    public int setShotSelect
    {
        set { shotSelect = value; }
    }
    void Start()
    {
        //�v���C���[�̃R���|�[�l���g
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //�J����
        mainCam = GameObject.Find("Main Camera");
        camera = mainCam.GetComponent<Camera>();
        followCam = mainCam.GetComponent<FollowCamera>();

        //�X�e�[�^�X
        isActive = true;
        hp = 225;
        speed = 0.05f;
        jumpPower = 280.0f;
        jumpCount = 0;

        hitFlg = false;
        damage = 0;
        hitCount = INVINCIBLE;

        //�e�֘A
        MagicArray = transform.Find("PlayerShot/MagicArray").gameObject;
        ShotObject = new GameObject[ShotType];
        ShotObject[0] = (GameObject)Resources.Load("shot_0");
        ShotObject[1] = (GameObject)Resources.Load("shot_0");
        ShotObject[2] = (GameObject)Resources.Load("shot_0");
        ShotObject[3] = (GameObject)Resources.Load("shot_0");
        shotSelect = 0;
        cooltime = 0;
        bulletCnt = 0;
        bulletCntFlg = false;
        bulletNumCnt = 0;

        //�}�E�X�N���b�N
        clickFlg = false;

        //�t���[���J�E���g
        count = 0;

        //�ǔ���
        _wallContact = new GameObject[2];
        scr_WallContact = new WallContact[2];
        _wallContact[0] = transform.Find("Contact_L").gameObject;
        _wallContact[1] = transform.Find("Contact_R").gameObject;
        scr_WallContact[0] = _wallContact[0].GetComponent<WallContact>();
        scr_WallContact[1] = _wallContact[1].GetComponent<WallContact>();

        //������
        _floorContact = transform.Find("Contact_Down").gameObject;
        scr_FloorContact = _floorContact.GetComponent<FloorContact>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {   //�A�N�e�B�u��Ԃ̎�
            Walk();             //����
            Jump();             //�W�����v
            Shot();             //�V���b�g
            Count();            //�J�E���g
            Damage();           //�_���[�W
            if (transform.position.y < -7) isActive = false;
        }
        else Debug.Log("aaa");

    }

    void Walk()
    {
        topLeft = camera.ScreenToWorldPoint(Vector3.zero);  //����̍��W
        topLeft.Scale(new Vector3(1f, -1f, 1f));

        bottomRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));    //�E���̍��W
        bottomRight.Scale(new Vector3(1f, -1f, 1f));

        if (Input.GetKey(KeyCode.A) && !scr_WallContact[0].getContact)
        {   //���ړ�
            transform.position -= new Vector3(speed, 0, 0);
            sr.flipX = true;
            anim.SetFloat("Speed", speed);
        }
        else if (Input.GetKey(KeyCode.D) && !scr_WallContact[1].getContact)
        {   //�E�ړ�
            transform.position += new Vector3(speed, 0, 0);
            sr.flipX = false;
            anim.SetFloat("Speed", speed);
        }
        else anim.SetFloat("Speed", -1);

        if (!followCam.GetMoveFlg)
        {   //�C�x���g���̈ړ�����
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, topLeft.x, bottomRight.x), transform.position.y);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && jumpCount < MaxJumpCount && 30 < count)
        {
            rb.velocity = Vector2.zero;             //���x���Z�b�g
            rb.AddForce(jumpPower * Vector2.up);    //�͂�������
            if(jumpCount == 0)jumpCount = 1;        //�J�E���g���Z
            if(jumpCount == 1)jumpCount = 2;        //�J�E���g���Z
            count = 0;                              //�W�����v�̃N�[���^�C�����Z�b�g
        }

        if (scr_FloorContact.getFloorContact)
        {
            jumpCount = 0;    //�W�����v�񐔃��Z�b�g
        }
    }

    void Shot()
    {
        if (Input.GetMouseButton(0))        //���N���b�N
        {

            if (CoolTime[shotSelect] < cooltime)
            {
                switch (shotSelect)
                {
                    case 0:
                        if (!clickFlg) Shot_0();
                        clickFlg = true;
                        break;
                    case 1:
                        if(!clickFlg) Shot_1();
                        clickFlg = true;
                        break;
                    case 2:
                        Shot_2();
                        break;
                    case 3:
                            if(!clickFlg) Shot_3();
                            if(!bulletCntFlg) bulletCntFlg = true;
                            break;
                }

                cooltime = 0;   //�N�[���^�C�����Z�b�g
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            clickFlg = false;
        }
    }

    void Count()
    {   //�J�E���g�n�֐�
        count++;
        cooltime++;
        hitCount--;
        if (60 < count) count = 60;
        if (60 < cooltime) cooltime = 60;
        if (hitCount < -1) hitCount = -1;
        if (bulletCntFlg) bulletCnt++;
        if (!bulletCntFlg) bulletCnt = 0;
    }

    void Shot_0()
    {   //�����V���b�g

        //����
        GameObject shot = Instantiate(ShotObject[shotSelect], MagicArray.transform.position, Quaternion.identity);

        // �N���b�N�������W�̎擾�i�X�N���[�����W���烏�[���h���W�ɕϊ��j
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // �����̐����iZ�����̏����Ɛ��K���j
        Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

        // �e�ɑ��x��^����
        shot.GetComponent<Rigidbody2D>().velocity = shotForward * ShotSpeed[shotSelect];

        // �e�̍U���͌���
        shot.GetComponent<Bullet>().power = ShotPower[shotSelect];
    }

    void Shot_1()
    {   //�P��

        Shot_0();
    }

    void Shot_2()
    {   //�A��

        Shot_0();
    }

    void Shot_3()
    {   //3�_�o�[�X�g
        int cooltime = 25;      //�N�[���^�C��
        int shotBulletNum = 3;  //�V���b�g��

        if (bulletCnt % 1 == 0 && bulletCnt < cooltime && bulletNumCnt < shotBulletNum)
        {   //���˃��[�g //�N�[���^�C�� //���ː�
            Shot_0();
            bulletNumCnt++;
        }
        if (shotBulletNum < bulletNumCnt) clickFlg = true;
        if (cooltime < bulletCnt)
        {
            bulletCntFlg = false;
            bulletNumCnt = 0;
            bulletCnt = 0;
        }
    }

    void Damage()
    {
        if (hitFlg && hitCount < 0)
        {   //�G�̍U�������������� �q�b�g�J�E���g��60�ȏ�̎�
            
            hp -= damage;            //�_���[�W
            hitFlg = false;          //�t���O���Z�b�g
            hitCount = INVINCIBLE;   //���G����
        }
        else
        {
            damage = 0;
        }

        if (hp <= 0) isActive = false;
    }

}
