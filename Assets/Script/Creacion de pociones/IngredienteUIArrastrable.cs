using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredienteUIArrastrable : MonoBehaviour, IPointerDownHandler
{
    public GameObject prefabIngredienteUI; // Este ser� el duplicado

    public ItemIngredientes itemIngrediente;
    public bool duplicar;

    void Start()
    {
        if(itemIngrediente !=null)
        GetComponent<Image>().sprite = itemIngrediente.imagen;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // Instanciar duplicado
        if(duplicar)
        {
            GameObject clon = Instantiate(prefabIngredienteUI, transform.position, Quaternion.identity, transform.parent.parent); // aseg�rate de que se coloque dentro del Canvas
            clon.name = prefabIngredienteUI.name;
            clon.AddComponent<ArrastrarUI>();
            clon.GetComponent<IngredienteUIArrastrable>().duplicar = false; // ESTA L�NEA es la clave
        }

    }
}
