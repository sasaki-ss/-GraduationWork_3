using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update

    private int itemNum;

    public Sprite img0;
    public Sprite img1;
    public Sprite img2;
    public Sprite img3;
    void Start()
    {

        //�����_���Ő������̃V���b�g�A�C�e���̎�ނ�����
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        //0�͏��������̂��ߏ��O 1�`3��Ԃ�
        itemNum = UnityEngine.Random.Range(1, 4);

        //�摜�ύX
        switch (itemNum)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = img0;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = img1;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = img2;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = img3;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //�v���C���[�̃V���b�g�I��ԍ������̃A�C�e���̔ԍ��ɍX�V
            collision.GetComponent<Player>().setShotSelect = itemNum;
            Destroy(gameObject);
        }
    }

}
