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

    // Script Lucy
    public static HashSet<string> clonesActivos = new HashSet<string>();
    //

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
            // Script Lucy
            if (clonesActivos.Contains(prefabIngredienteUI.name))
            {
                Debug.Log("Ya existe un clon activo de " + prefabIngredienteUI.name);
                return;
            }
            //

            GameObject clon = Instantiate(prefabIngredienteUI, transform.position, Quaternion.identity, transform.parent.parent); // aseg�rate de que se coloque dentro del Canvas
            clon.name = prefabIngredienteUI.name;
            clon.AddComponent<ArrastrarUI>();
            clon.GetComponent<IngredienteUIArrastrable>().duplicar = false; // ESTA L�NEA es la clave

            // Script Lucy
            clonesActivos.Add(prefabIngredienteUI.name);
            //
        }


    }
}
