using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power { get; set; }  //�U���͕ێ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {   //�J�����O�ɏo���ꍇ�폜
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Untagged")
        {   //�^�O���ݒ�̃I�u�W�F�N�g�ɂ͓������Ă��폜���Ȃ�
            Destroy(gameObject);
        }
    }
}
