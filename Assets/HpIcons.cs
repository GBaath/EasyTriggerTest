using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpIcons : MonoBehaviour
{
    public Image[] images;

    public void UpdateHp(int hp)
    {
        //show amount of hp icons
        foreach (var image in images) 
        {
            image.enabled = false;
        }
        for(int i = 0; i < hp; i++) 
        {
            images[i].enabled = true;
        }
    }
}
