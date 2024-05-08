using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public Image healthBar;
    public Image manaBar;

 
    public void UpdateHealth(float healthRatio)
    {
        if(healthRatio <= 0)
        {
            healthRatio = 0;
        } 

        healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);
    }

    public void UpdateMana(float manaRatio)
    {
        if (manaRatio <= 0)
        {
            manaRatio = 0;
        }

        manaBar.transform.localScale = new Vector3(manaRatio, 1, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
