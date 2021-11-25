using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContact : MonoBehaviour
{
    bool Contact;

    public bool getContact
    {
        get { return Contact; }
    }

    void Start()
    {
        Contact = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Floor" || collision.tag == "Wall")
        {
            Contact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Contact = false;
    }
}
