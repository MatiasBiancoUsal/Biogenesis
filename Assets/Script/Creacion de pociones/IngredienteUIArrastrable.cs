using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredienteUIArrastrable : MonoBehaviour, IPointerDownHandler
{
    public GameObject prefabIngredienteUI; // Este será el duplicado

    public Items item;
    public bool duplicar;

    void Start()
    {
        if(item !=null)
        GetComponent<Image>().sprite = item.imagen;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // Instanciar duplicado
        if(duplicar)
        {
            GameObject clon = Instantiate(prefabIngredienteUI, transform.position, Quaternion.identity, transform.parent.parent); // asegúrate de que se coloque dentro del Canvas
            clon.name = prefabIngredienteUI.name;
            clon.AddComponent<ArrastrarUI>();
        }

    }
}
