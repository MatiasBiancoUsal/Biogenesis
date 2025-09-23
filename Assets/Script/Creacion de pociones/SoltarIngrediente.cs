using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Asegúrate de tener esta línea para Image y CanvasGroup

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;
    private List<GameObject> ingredientesUI = new List<GameObject>();

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    public GameObject prefabPocionVida;
    public GameObject prefabPocionMejora;

    // Cooldown
    public float cooldownVida = 10f;
    public float cooldownMejora = 10f;
    private static float tiempoRestanteVida = 0f;
    private static float tiempoRestanteMejora = 0f;

    // Timers para el log de la consola
    private float contadorLogVida = 0f;
    private float contadorLogMejora = 0f;

    [SerializeField] private Transform panelIngredientes; // Arrastra el panel en el inspector

    void Awake()
    {
        // Recuperar cooldowns guardados al iniciar la aplicación
        tiempoRestanteVida = PlayerPrefs.GetFloat("CooldownVida", 0f);
        tiempoRestanteMejora = PlayerPrefs.GetFloat("CooldownMejora", 0f);
    }

    void OnDestroy()
    {
        // Guardar cooldowns actuales al salir de la escena o cerrar el juego
        PlayerPrefs.SetFloat("CooldownVida", tiempoRestanteVida);
        PlayerPrefs.SetFloat("CooldownMejora", tiempoRestanteMejora);
        PlayerPrefs.Save();
    }

    void Start()
    {
        // Si al entrar a la escena hay un cooldown activo, bloquear los ingredientes
        if (tiempoRestanteVida > 0)
        {
            BloquearIngredientes(TipoPocion.Vida);
        }

        if (tiempoRestanteMejora > 0)
        {
            BloquearIngredientes(TipoPocion.Mejora);
        }
    }

    void Update()
    {
        // Reducir el tiempo restante de los cooldowns cada fotograma
        if (tiempoRestanteVida > 0)
        {
            tiempoRestanteVida -= Time.deltaTime;
            contadorLogVida += Time.deltaTime;
            if (contadorLogVida >= 1f)
            {
                Debug.Log("Cooldown VIDA: " + Mathf.Ceil(tiempoRestanteVida) + "s");
                contadorLogVida = 0f;
            }

            if (tiempoRestanteVida <= 0)
            {
                tiempoRestanteVida = 0; // Asegurarse de que no sea negativo
                DesbloquearIngredientes(TipoPocion.Vida);
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

            if (tiempoRestanteMejora <= 0)
            {
                tiempoRestanteMejora = 0; // Asegurarse de que no sea negativo
                DesbloquearIngredientes(TipoPocion.Mejora);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject objetoArrastrado = eventData.pointerDrag;
        if (objetoArrastrado == null) return;

        bool esVida = objetoArrastrado.name.Contains("VidaA") || objetoArrastrado.name.Contains("VidaB");
        bool esMejora = objetoArrastrado.name.Contains("MejoraA") || objetoArrastrado.name.Contains("MejoraB");

        if ((esVida && EstaEnCooldown(TipoPocion.Vida)) || (esMejora && EstaEnCooldown(TipoPocion.Mejora)))
        {
            Debug.Log("No puedes usar este ingrediente, el cooldown está activo.");
            return; // Bloquea el drop
        }

        ingredientesEnZona.Add(objetoArrastrado.name);
        ingredientesUI.Add(objetoArrastrado);
        objetoArrastrado.transform.SetParent(transform, false);
        RevisarCombinaciones();
    }

    void RevisarCombinaciones()
    {
        bool combinacionVida = ingredientesEnZona.Contains("VidaA") && ingredientesEnZona.Contains("VidaB");
        bool combinacionMejora = ingredientesEnZona.Contains("MejoraA") && ingredientesEnZona.Contains("MejoraB");

        if (combinacionVida)
        {
            Debug.Log("¡Combinación de VIDA detectada!");
            IniciarCooldown(TipoPocion.Vida);
            CrearPocion(pocionVidaPrefab);
            return;
        }

        if (combinacionMejora)
        {
            Debug.Log("¡Combinación de MEJORA detectada!");
            IniciarCooldown(TipoPocion.Mejora);
            CrearPocion(pocionMejoraPrefab);
            return;
        }

        // Si se sueltan dos ingredientes pero no son una combinación válida
        if (ingredientesEnZona.Count > 1)
        {
            Debug.Log("Ingredientes no compatibles, eliminándolos.");
            foreach (GameObject ingrediente in ingredientesUI)
            {
                // Esta es la línea importante que agregaste para gestionar los clones
                IngredienteUIArrastrable.clonesActivos.Remove(
                    ingrediente.name.Replace("(Clone)", "").Trim()
                );
                Destroy(ingrediente);
            }
            ingredientesUI.Clear();
            ingredientesEnZona.Clear();
        }
    }

    void CrearPocion(GameObject prefab)
    {
        // Instanciar la poción y configurarla en la UI
        GameObject nuevaPocion = Instantiate(prefab, transform);
        nuevaPocion.transform.SetParent(transform, false); // false para mantener escala/posición
        if (nuevaPocion.GetComponent<RectTransform>() != null)
        {
            nuevaPocion.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centrarla
        }

        // Asignar el prefab correcto al script AgarrarPocion
        var agarrarPocion = nuevaPocion.GetComponent<AgarrarPocion>();
        if (agarrarPocion != null)
        {
            if (prefab == pocionVidaPrefab)
                agarrarPocion.prefabParaMaletin = prefabPocionVida;
            else if (prefab == pocionMejoraPrefab)
                agarrarPocion.prefabParaMaletin = prefabPocionMejora;
        }

        Debug.Log("Poción creada correctamente.");

        // Eliminar los ingredientes usados
        foreach (GameObject ingrediente in ingredientesUI)
        {
            IngredienteUIArrastrable.clonesActivos.Remove(
                ingrediente.name.Replace("(Clone)", "").Trim()
            );
            Destroy(ingrediente);
        }

        // Limpiar las listas
        ingredientesUI.Clear();
        ingredientesEnZona.Clear();
    }

    public enum TipoPocion { Vida, Mejora }

    public bool EstaEnCooldown(TipoPocion tipo)
    {
        if (tipo == TipoPocion.Vida)
        {
            return tiempoRestanteVida > 0;
        }
        else
        {
            return tiempoRestanteMejora > 0;
        }
    }

    public void IniciarCooldown(TipoPocion tipo)
    {
        if (tipo == TipoPocion.Vida)
        {
            tiempoRestanteVida = cooldownVida;
            BloquearIngredientes(TipoPocion.Vida);
        }
        else if (tipo == TipoPocion.Mejora)
        {
            tiempoRestanteMejora = cooldownMejora;
            BloquearIngredientes(TipoPocion.Mejora);
        }
    }

    private void BloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null)
        {
            Debug.LogError("Panel de ingredientes no asignado en el inspector.");
            return;
        }

        foreach (Transform t in panelIngredientes)
        {
            bool esVida = tipo == TipoPocion.Vida && (t.name.Contains("VidaA") || t.name.Contains("VidaB"));
            bool esMejora = tipo == TipoPocion.Mejora && (t.name.Contains("MejoraA") || t.name.Contains("MejoraB"));

            if (esVida || esMejora)
            {
                var cg = t.GetComponent<CanvasGroup>() ?? t.gameObject.AddComponent<CanvasGroup>();
                cg.blocksRaycasts = false;
                cg.interactable = false;

                var img = t.GetComponent<Image>();
                if (img != null)
                {
                    img.color = new Color(0.5f, 0.5f, 0.5f, img.color.a); // Color gris
                }
            }
        }
    }

    private void DesbloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null)
        {
            Debug.LogError("Panel de ingredientes no asignado en el inspector.");
            return;
        }

        foreach (Transform t in panelIngredientes)
        {
            bool esVida = tipo == TipoPocion.Vida && (t.name.Contains("VidaA") || t.name.Contains("VidaB"));
            bool esMejora = tipo == TipoPocion.Mejora && (t.name.Contains("MejoraA") || t.name.Contains("MejoraB"));

            if (esVida || esMejora)
            {
                var cg = t.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.blocksRaycasts = true;
                    cg.interactable = true;
                }

                var img = t.GetComponent<Image>();
                if (img != null)
                {
                    img.color = new Color(1f, 1f, 1f, img.color.a); // Color original (blanco)
                }
            }
        }
    }
}