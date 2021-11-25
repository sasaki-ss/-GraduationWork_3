using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorContact : MonoBehaviour
{
    bool floorContact;

    public bool getFloorContact
    {
        get { return floorContact; }
    }
    private void Start()
    {
        floorContact = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            floorContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        floorContact = false;
    }
}
