using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class hpText : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    public void updateText(int maxHp, int currentHp)
    {
        string text = currentHp + "/" + maxHp + "Hp";
        textMeshPro.SetText(text);
    } 
}
