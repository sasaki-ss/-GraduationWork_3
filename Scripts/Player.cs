using UnityEngine;

public class Player : MonoBehaviour
{
    //�v���C���[�̃R���|�[�l���g
    SpriteRenderer sr;
    Rigidbody2D rb;
    Animator anim;

    //�X�e�[�^�X
    private float speed;        //�ړ����x
    private float jumpPower;    //�W�����v�̍���
    private int jumpCount;      //�W�����v�񐔂̃J�E���g

    //�e�֘A
    private GameObject MagicArray;  //���@�w
    private GameObject shot_0;      //�e
    private int shot_speed;         //�e�̑��x
    private int cooltime;           //�e���˂̃N�[���^�C��
    

    //�萔
    private const int MaxJumpCount = 2; //�ő�W�����v��
    private const int CoolTime0 = 5;    //shot_0�̃N�[���^�C��

    //�t���[���J�E���g
    private int count;

    //�ǔ���
    GameObject[] _wallContact;
    WallContact[] scr_WallContact;

    //������
    GameObject _floorContact;
    FloorContact scr_FloorContact;

    void Start()
    {
        //�v���C���[�̃R���|�[�l���g
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //�X�e�[�^�X
        speed = 0.05f;
        jumpPower = 280.0f;
        jumpCount = 0;

        //�e�֘A
        MagicArray = transform.Find("PlayerShot/MagicArray").gameObject;
        shot_0 = (GameObject)Resources.Load("shot_0");
        shot_speed = 20;
        cooltime = 0;

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
        Walk();
        Jump();
        Shot();
        count++;
        cooltime++;
    }

    void Walk()
    {
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
            if (CoolTime0 < cooltime)
            {
                //����
                GameObject shot = Instantiate(shot_0, MagicArray.transform.position, Quaternion.identity);

                // �N���b�N�������W�̎擾�i�X�N���[�����W���烏�[���h���W�ɕϊ��j
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // �����̐����iZ�����̏����Ɛ��K���j
                Vector3 shotForward = Vector3.Scale((mouseWorldPos - transform.position), new Vector3(1, 1, 0)).normalized;

                // �e�ɑ��x��^����
                shot.GetComponent<Rigidbody2D>().velocity = shotForward * shot_speed;

                cooltime = 0;   //�N�[���^�C�����Z�b�g
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
