using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stageObj;

    private int nowOnPlayer;

    //����������
    private void Start()
    {
        nowOnPlayer = 0;

        foreach (GameObject obj in stageObj)
        {
            obj.SetActive(false);
        }

        stageObj[nowOnPlayer].SetActive(true);
        stageObj[nowOnPlayer + 1].SetActive(true);
    }

    //�X�V����
    private void Update()
    {

    }

    //�\���G���A�̐ؑ֏���
    public void ChangeArea(int _areaNum)
    {
        if (nowOnPlayer == _areaNum) return;

        nowOnPlayer = _areaNum;
        if (nowOnPlayer == 0)
        {
            stageObj[nowOnPlayer + 1].SetActive(true);
        }
        else if (nowOnPlayer == stageObj.Length - 1)
        {
            stageObj[nowOnPlayer - 1].SetActive(true);
        }
        else
        {
            stageObj[nowOnPlayer + 1].SetActive(true);
            stageObj[nowOnPlayer - 1].SetActive(true);
        }

        foreach (GameObject obj in stageObj)
        {
            int areaNum = obj.GetComponent<Area>().GetAreaNum();

            if (areaNum == nowOnPlayer || areaNum == nowOnPlayer + 1||
                areaNum == nowOnPlayer - 1)
            {
                continue;
            }

            obj.SetActive(false);
        }

        Debug.Log("Change!!");
    }
}
