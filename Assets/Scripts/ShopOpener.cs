using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpener : MonoBehaviour
{
    private GameObject shopUI;

    // Start is called before the first frame update
    void Start()
    {
        if (shopUI == null) shopUI = GameObject.FindWithTag("shop");
        shopUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shopUI.SetActive(false);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shopUI.SetActive(true);
        }
    }
}