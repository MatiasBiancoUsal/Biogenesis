using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;

    private List<GameObject> ingredientesUI = new List<GameObject>();

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    public GameObject prefabPocionVida;
    public GameObject prefabPocionMejora;

    // Script Lucy
    public float cooldownSegundos = 10f; 
    private bool enCooldown = false;
    private float cooldownEndTime = 10f;
    //

    public void OnDrop(PointerEventData eventData)
    {
        // Script Lucy
        if (enCooldown)
        {
            float restante = Mathf.Max(0f, cooldownEndTime - Time.time);
            Debug.Log($"Debes esperar {restante:F1}s antes de crear otra poción.");
            return;
        }
        //

        Debug.Log("Algo fue soltado en la zona"); // Ver si llega hasta acá

        GameObject objetoArrastrado = eventData.pointerDrag;

        if (objetoArrastrado != null)
        {
            Debug.Log("Se soltó: " + objetoArrastrado.name);

            ingredientesEnZona.Add(objetoArrastrado.name);

            ingredientesUI.Add(objetoArrastrado); //para guardar los gameobject clonados

            objetoArrastrado.transform.SetParent(transform, false);

            RevisarCombinaciones();
        }
    }

    void RevisarCombinaciones()
    {
        bool combinacionVida = ingredientesEnZona.Contains("VidaA") && ingredientesEnZona.Contains("VidaB");
        bool combinacionMejora = ingredientesEnZona.Contains("MejoraA") && ingredientesEnZona.Contains("MejoraB");

        if (combinacionVida)
        {
            Debug.Log("¡Combinación de VIDA detectada!");
            CrearPocion(pocionVidaPrefab);
            return;

            
            //// Evitar seguir revisando si ya se creó una poción
        }

        if (combinacionMejora)
        {
            Debug.Log("¡Combinación de MEJORA detectada!");
            CrearPocion(pocionMejoraPrefab);
            return;

        }

        // Si no hay combinación válida, limpiar los ingredientes (destruirlos)
        if (ingredientesEnZona.Count > 1)
        {
            Debug.Log("Ingredientes no compatibles, eliminándolos.");
            foreach (GameObject ingrediente in ingredientesUI)
            {
                Destroy(ingrediente);
            }

            ingredientesUI.Clear();
            ingredientesEnZona.Clear();

           
        }
        


    }

    void CrearPocion(GameObject prefab)
    {
        GameObject nuevaPocion = Instantiate(prefab, transform); // Esto la pone como hija de ZonaDeFusion
        nuevaPocion.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centrarla si es UI

        // Si estás usando UI, esto es importante:
        nuevaPocion.transform.SetParent(transform, false); // false para mantener escala/posición del prefab

        // Asignar el prefab para maletín desde aquí
        var agarrarPocion = nuevaPocion.GetComponent<AgarrarPocion>();
        if (agarrarPocion != null)
        {
            if (prefab == prefabPocionVida)
                agarrarPocion.prefabParaMaletin = prefabPocionVida;
            else if (prefab == prefabPocionMejora)
                agarrarPocion.prefabParaMaletin = prefabPocionMejora;
        }

        Debug.Log("Poción creada correctamente.");

        // Eliminar visualmente los ingredientes usados
        foreach (GameObject ingrediente in ingredientesUI)
        {
            Destroy(ingrediente);
        }

        // Limpiar las listas
        ingredientesUI.Clear();
        ingredientesEnZona.Clear();

        // Script Lucy
        if (!enCooldown)
            StartCoroutine(IniciarCooldown());
        //
    }

    // Script Lucy
    IEnumerator IniciarCooldown()
    {
        enCooldown = true;
        cooldownEndTime = Time.time + cooldownSegundos;
        // si querés ver logs cada segundo:
        float restante = cooldownSegundos;
        while (restante > 0f)
        {
            Debug.Log($"Cooldown: {restante:F1}s");
            yield return new WaitForSeconds(1f);
            restante -= 1f;
        }
        enCooldown = false;
        Debug.Log("Ya puedes crear otra poción.");
    }

    // Helper para sacar "(Clone)" y espacios
    string GetBaseName(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }
    //
}
