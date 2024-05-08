using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckSphere : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isGrounded = true;
        player.HitTheGround();
    }
}
