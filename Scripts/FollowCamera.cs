using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject Player;
    private float tempX;      //移動をスムーズに行うための座標格納
    private float moveVel;      //移動速度
    private bool moveFlg;       //動くフラグ

public bool GetMoveFlg
    {
        get { return moveFlg; }
    }

    public bool SetMoveFlg
    {
        set { moveFlg = value; }
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        moveVel = 0.15f;
        moveFlg = true;
    }

    // Update is called once per frame
    void Update()
    {
        //カメラ追従
        //transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

        //徐々にカメラ追従
        if (moveFlg && Player.transform.position.y < 2.5)
        {
            tempX = Mathf.SmoothStep(transform.position.x, Player.transform.position.x, moveVel);
            transform.position = new Vector3(tempX, transform.position.y, transform.position.z);
        }

        else if (moveFlg && 2.5 < Player.transform.position.y)
        {
            tempX = Mathf.SmoothStep(transform.position.x, Player.transform.position.x, moveVel);
            transform.position = new Vector3(tempX, Player.transform.position.y - 2.5f, transform.position.z);
        }

    }
}
