using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    public AudioClip soundClip;
    public AudioSource source;
    private Transform other;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
    }

    public void Track(Transform other)
    {
        this.other = other;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (other != null)
        {
            transform.position = other.forward * 2 + other.position;
            transform.rotation = other.rotation;
        }
    }


    IEnumerator DestroySelf()
    {
        int x = 0;
        while(x < 25)
        {
            x++;
            source.PlayOneShot(soundClip);
            yield return new WaitForSeconds(0.06f);
        }
        
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }
}
