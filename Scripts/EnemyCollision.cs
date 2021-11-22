using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private bool isEnter;   //侵入フラグ
    private bool isStay;    //侵入中フラグ
    private bool isExit;    //退出フラグ

    public bool isInvasion { get; private set; }  //攻撃フラグ

    private void Start()
    {
        isEnter = false;
        isStay = false;
        isExit = false;

        isInvasion = false;
    }

    private void Update()
    {
        isInvasion = false;

        if (isEnter || isStay)
        {
            isInvasion = true;
        }

        if (isEnter)
        {
            Debug.Log("侵入しました");
            isEnter = false;
        }

        if (isStay)
        {
            Debug.Log("侵入中");
        }

        if (isExit)
        {
            Debug.Log("エリアから退出しました");
            isExit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isEnter = true;
            isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isExit = true;
            isStay = false;
        }
    }
}
