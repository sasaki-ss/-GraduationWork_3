using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    public bool isFloor { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isFloor = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isFloor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isFloor = false;
        }
    }
}
