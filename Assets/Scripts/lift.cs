using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Netcode;
using UnityEngine;

public class lift : NetworkBehaviour
{
    public float speed;
    public float maxHeight;
    public float minHeight;
    public bool isAscending = true;

    public bool liftActive = false;
    public bool isOnCooldown = false;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(liftActive)
        {
            if(isAscending) transform.position += Vector3.up * speed * Time.deltaTime;
            else transform.position -= Vector3.up * speed * Time.deltaTime;

            if (transform.position.y >= maxHeight)
            {
                isAscending = false;
                liftActive = false;
                isOnCooldown = true;

                player.transform.parent = null;
                transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);

                StartCoroutine(RecoverCooldown());
            }
            else if(transform.position.y <= minHeight)
            {
                isAscending = true;
                liftActive = false;
                player.transform.parent = null;
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
                isOnCooldown = true;
                StartCoroutine(RecoverCooldown());
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            if (liftActive == false) StartCoroutine(TurnOnLift(player));
        }
    }

    IEnumerator TurnOnLift(GameObject Player)
    {
        yield return new WaitForSeconds(0.5f);

        if (!isOnCooldown)
        {
            player.transform.parent = gameObject.transform;
            liftActive = true;
            isOnCooldown = true;
        }
    }

    IEnumerator RecoverCooldown()
    {
        yield return new WaitForSeconds(1f);
        isOnCooldown = false;
    }


}
