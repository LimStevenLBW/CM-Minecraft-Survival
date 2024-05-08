using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATACK : MonoBehaviour
{
    public Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            Vector3 direction = other.gameObject.transform.position - playerTransform.position;


            enemy.TakeDamage(10, direction);
        }
    }

}
