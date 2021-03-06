using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int power { get; set; }  //攻撃力保持

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {   //カメラ外に出た場合削除
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Untagged" && collision.tag != "Bullet")
        {   //タグ未設定のオブジェクトには当たっても削除しない
            //弾同士も当たっても削除しない
            Destroy(gameObject);
        }
    }
}
