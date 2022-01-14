using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float   moveSpeed;  //移動速度
    private Vector3 velocity;   //速度

    //初期化処理
    private void Start()
    {
        moveSpeed = 0.2f;
    }

    //更新処理
    private void Update()
    {
        //座標の更新
        this.transform.position += velocity * moveSpeed;

        //画面外に出た際このオブジェクトを消去
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    //初期化処理
    public void Init(float val)
    {
        //プレイヤーと火球間の単位ベクトルを取得
        GameObject player = GameObject.Find("Player");
        Vector3 twoPointVec = player.transform.position - this.transform.position;
        velocity = twoPointVec.normalized;

        //プレイヤーがドラゴンより右側にいる際反転
        if(val > 0)
        {
            transform.localScale = new Vector3(0.1f, -0.1f, 1f);
        }
    }

    //当たり判定処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        //当たったオブジェクトがプレイヤーの際
        if(other.gameObject.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.SetHitFlg = true;
            player.SetDamage = 50;

            Destroy(this.gameObject);
        }
    }
}
