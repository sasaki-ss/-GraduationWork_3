using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameObject parentObj = this.transform.root.gameObject;
            parentObj.GetComponent<
                Enemy>().Damage(other.gameObject.GetComponent<Bullet>().power);
        }
    }
}
