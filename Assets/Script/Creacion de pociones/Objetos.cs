using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos : MonoBehaviour
{
    public ItemIngredientes itemIngrediente;


    void Start()
    {
        if (itemIngrediente != null)
            GetComponent<SpriteRenderer>().sprite = itemIngrediente.imagen;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
