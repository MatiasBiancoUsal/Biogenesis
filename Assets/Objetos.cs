using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos : MonoBehaviour
{
    public Items item;


    void Start()
    {
        if (item != null)
            GetComponent<SpriteRenderer>().sprite = item.imagen;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
