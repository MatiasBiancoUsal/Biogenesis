using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Algo fue soltado en la zona"); // Ver si llega hasta acá

        GameObject objetoArrastrado = eventData.pointerDrag;

        if (objetoArrastrado != null)
        {
            Debug.Log("Se soltó: " + objetoArrastrado.name);

            ingredientesEnZona.Add(objetoArrastrado.name);

            objetoArrastrado.transform.SetParent(transform, false);

            RevisarCombinaciones();
        }
    }

    void RevisarCombinaciones()
    {
        Debug.Log("Revisando combinaciones...");

        // VIDA
        if (ingredientesEnZona.Contains("VidaA") && ingredientesEnZona.Contains("VidaB"))
        {
            Debug.Log("¡Combinación de VIDA detectada!");
            CrearPocion(pocionVidaPrefab);
        }

        // MEJORA
        if (ingredientesEnZona.Contains("MejoraA") && ingredientesEnZona.Contains("MejoraB"))
        {
            Debug.Log("¡Combinación de MEJORA detectada!");
            CrearPocion(pocionMejoraPrefab);
        }
    }

    void CrearPocion(GameObject prefab)
    {
        GameObject nuevaPocion = Instantiate(prefab, transform); // Esto la pone como hija de ZonaDeFusion
        nuevaPocion.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centrarla si es UI

        // Si estás usando UI, esto es importante:
        nuevaPocion.transform.SetParent(transform, false); // false para mantener escala/posición del prefab

        Debug.Log("Poción creada correctamente.");
    }
}
