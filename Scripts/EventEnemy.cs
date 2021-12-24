using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemy : MonoBehaviour
{
    private void OnDestroy()
    {
        GameObject.Find("EventManager").GetComponent<
            EventManager>().Defeat();
    }
}
