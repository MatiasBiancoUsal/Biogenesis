using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursormouse : MonoBehaviour
{
    SpriteRenderer imagecolor;
    public Color gris = Color.gray;
    public Color blanco = Color.white;

    private void OnMouseOver()
    {
        imagecolor = GetComponent<SpriteRenderer>();   
        imagecolor.color = gris;
    }

    private void OnMouseExit()
    {
        imagecolor = GetComponent<SpriteRenderer>();
        imagecolor.color = blanco;
    }

}
