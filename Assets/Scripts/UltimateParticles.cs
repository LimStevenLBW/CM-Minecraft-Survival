using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateParticles : MonoBehaviour
{
    public ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    public void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            Vector3 direction = other.gameObject.transform.position - transform.position;


            enemy.TakeDamage(5, direction);
        }

    }
}
