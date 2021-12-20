using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject Player;
    private float tempX;      //�ړ����X���[�Y�ɍs�����߂̍��W�i�[
    private float moveVel;      //�ړ����x
    void Start()
    {
        Player = GameObject.Find("Player");
        moveVel = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        //�J�����Ǐ]
        //transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);

        //���X�ɃJ�����Ǐ]
        tempX = Mathf.SmoothStep(transform.position.x, Player.transform.position.x,moveVel);
        transform.position = new Vector3(tempX, transform.position.y, transform.position.z);
    }
}
