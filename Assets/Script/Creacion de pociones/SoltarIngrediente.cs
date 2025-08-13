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
    public float cooldownVida = 10f;
    public float cooldownMejora = 10f;

    private static float tiempoRestanteVida = 0f;
    private static float tiempoRestanteMejora = 0f;
    //

    // Script Lucy
    
    private float contadorLogVida = 0f;
    private float contadorLogMejora = 0f;

    void Update()
    {
        // Reducir los timers globales
        if (tiempoRestanteVida > 0)
        {
            tiempoRestanteVida -= Time.deltaTime;
            contadorLogVida += Time.deltaTime;
            if (contadorLogVida >= 1f)
            {
                Debug.Log("Cooldown VIDA: " + Mathf.Ceil(tiempoRestanteVida) + "s");
                contadorLogVida = 0f;
            }
        }

        if (tiempoRestanteMejora > 0)
        {
            tiempoRestanteMejora -= Time.deltaTime;
            contadorLogMejora += Time.deltaTime;
            if (contadorLogMejora >= 1f)
            {
                Debug.Log("Cooldown MEJORA: " + Mathf.Ceil(tiempoRestanteMejora) + "s");
                contadorLogMejora = 0f;
            }
        }
    }

    //
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Algo fue soltado en la zona"); // Ver si llega hasta ac�

        GameObject objetoArrastrado = eventData.pointerDrag;

        if (objetoArrastrado != null)
        {
            // Script Lucy
            bool esVida = objetoArrastrado.name.Contains("VidaA") || objetoArrastrado.name.Contains("VidaB");
            bool esMejora = objetoArrastrado.name.Contains("MejoraA") || objetoArrastrado.name.Contains("MejoraB");

            if (esVida && EstaEnCooldown(TipoPocion.Vida))
            {
                Debug.Log("No puedes usar ingredientes de VIDA, cooldown activo.");
                return; // bloquea el drop
            }
            if (esMejora && EstaEnCooldown(TipoPocion.Mejora))
            {
                Debug.Log("No puedes usar ingredientes de MEJORA, cooldown activo.");
                return; // bloquea el drop
            }
            //
            Debug.Log("Se solt�: " + objetoArrastrado.name);

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
            if (!EstaEnCooldown(TipoPocion.Vida))
            {
                Debug.Log("�Combinaci�n de VIDA detectada!");
                IniciarCooldown(TipoPocion.Vida); // Activar cooldown
                CrearPocion(pocionVidaPrefab);
            }
            else
            {
                Debug.Log("Poci�n de VIDA en cooldown.");
            }
            return;
        }

        if (combinacionMejora)
        {
            if (!EstaEnCooldown(TipoPocion.Mejora))
            {
                Debug.Log("�Combinaci�n de MEJORA detectada!");
                IniciarCooldown(TipoPocion.Mejora); // Activar cooldown
                CrearPocion(pocionMejoraPrefab);
            }
            else
            {
                Debug.Log("Poci�n de MEJORA en cooldown.");
            }
            return;

        }

        // Si no hay combinaci�n v�lida, limpiar los ingredientes (destruirlos)
        if (ingredientesEnZona.Count > 1)
        {
            Debug.Log("Ingredientes no compatibles, elimin�ndolos.");
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

        // Si est�s usando UI, esto es importante:
        nuevaPocion.transform.SetParent(transform, false); // false para mantener escala/posici�n del prefab

        // Asignar el prefab para malet�n desde aqu�
        var agarrarPocion = nuevaPocion.GetComponent<AgarrarPocion>();
        if (agarrarPocion != null)
        {
            if (prefab == prefabPocionVida)
                agarrarPocion.prefabParaMaletin = prefabPocionVida;
            else if (prefab == prefabPocionMejora)
                agarrarPocion.prefabParaMaletin = prefabPocionMejora;
        }

        Debug.Log("Poci�n creada correctamente.");

        // Eliminar visualmente los ingredientes usados
        foreach (GameObject ingrediente in ingredientesUI)
        {
            Destroy(ingrediente);
        }

        // Limpiar las listas
        ingredientesUI.Clear();
        ingredientesEnZona.Clear();
    }

    // Script Lucy
    public enum TipoPocion { Vida, Mejora }

    public bool EstaEnCooldown(TipoPocion tipo)
    {
        if (tipo == TipoPocion.Vida)
            return tiempoRestanteVida > 0;
        else
            return tiempoRestanteMejora > 0;
    }

    public void IniciarCooldown(TipoPocion tipo)
    {
        if (tipo == TipoPocion.Vida)
        {
            tiempoRestanteVida = cooldownVida;
            
        }
        else
        {
            tiempoRestanteMejora = cooldownMejora;
          
        }
    }
    //

    // Helper para sacar "(Clone)" y espacios
    string GetBaseName(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }
    //
}
