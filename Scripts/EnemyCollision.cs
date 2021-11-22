using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private bool isEnter;   //�N���t���O
    private bool isStay;    //�N�����t���O
    private bool isExit;    //�ޏo�t���O

    public bool isInvasion { get; private set; }  //�U���t���O

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
            Debug.Log("�N�����܂���");
            isEnter = false;
        }

        if (isStay)
        {
            Debug.Log("�N����");
        }

        if (isExit)
        {
            Debug.Log("�G���A����ޏo���܂���");
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
