using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredienteUIArrastrable : MonoBehaviour, IPointerDownHandler
{
    public GameObject prefabIngredienteUI; // Este será el duplicado

    public void OnPointerDown(PointerEventData eventData)
    {
        // Instanciar duplicado
        GameObject clon = Instantiate(prefabIngredienteUI, transform.position, Quaternion.identity, transform.parent.parent); // asegúrate de que se coloque dentro del Canvas
        clon.name = prefabIngredienteUI.name;
        clon.AddComponent<ArrastrarUI>();
    }
}
