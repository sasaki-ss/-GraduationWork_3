using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private EventType eventNum;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Find("EventManager").GetComponent
                <EventManager>().StartRunEvent(eventNum);

            GameObject.Find("Main Camera").GetComponent<
                FollowCamera>().SetMoveFlg = false;

            this.gameObject.SetActive(false);
        }
    }
}
