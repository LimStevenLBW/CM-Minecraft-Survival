using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public bool isPlayerOwned = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null && isPlayerOwned)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Vector3 direction = other.gameObject.transform.position - transform.forward;
            enemy.TakeDamage(10, direction);
        }
        else if (other.gameObject.GetComponent<Player>() != null && !isPlayerOwned){
            Player player = other.gameObject.GetComponent<Player>();
            Vector3 direction = other.gameObject.transform.position - transform.forward;
            player.TakeDamage(10, direction);
        }
    }
}
