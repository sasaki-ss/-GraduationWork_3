using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitChecker : MonoBehaviour
{
    [SerializeField]
    GameObject parentObj;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            parentObj.GetComponent<
                Enemy>().Damage(other.gameObject.GetComponent<Bullet>().power);
        }
    }
}
