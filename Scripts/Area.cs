using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField]
    private int areaNum;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Find("Stage").GetComponent<Stage>().ChangeArea(areaNum);
        }
    }

    public int GetAreaNum()
    {
        return areaNum;
    }
}
