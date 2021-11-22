using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private bool isEnter;
    private bool isStay;
    private bool isExit;

    public bool isAttack { get; private set; }

    private void Start()
    {
        isEnter = false;
        isStay = false;
        isExit = false;

        isAttack = false;
    }

    private void Update()
    {
        isAttack = false;

        if (isEnter || isStay || isExit)
        {
            isAttack = true;
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
