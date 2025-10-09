using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;
    private List<GameObject> ingredientesUI = new List<GameObject>();

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    public GameObject prefabPocionVida;
    public GameObject prefabPocionMejora;

    [Header("Cooldown")]
    public float cooldownVida = 10f;
    public float cooldownMejora = 10f;

    private const string CooldownEndTimeVidaKey = "CooldownEndTimeVida";
    private const string CooldownEndTimeMejoraKey = "CooldownEndTimeMejora";

    [Header("Asignaciones de UI")]
    [SerializeField] private Transform panelIngredientes;

    private float logTimerVida = 0f;
    private float logTimerMejora = 0f;

    void Start()
    {
        // Al iniciar la escena, actualiza el estado visual de los ingredientes inmediatamente.
        ActualizarEstadoVisual();
    }

    void Update()
    {
        // Revisa constantemente los cooldowns para actualizar la UI y los logs.
        ActualizarCooldowns();
    }

    private void ActualizarCooldowns()
    {
        // Lógica para Poción de Vida
        if (EstaEnCooldown(TipoPocion.Vida))
        {
            logTimerVida += Time.deltaTime;
            if (logTimerVida >= 1f)
            {
                int tiempoRedondeado = Mathf.CeilToInt(GetTiempoRestante(TipoPocion.Vida));
                Debug.Log("Tiempo restante para Poción de Vida: " + tiempoRedondeado + "s");
                logTimerVida = 0f;
            }
        }

        // Lógica para Poción de Mejora
        if (EstaEnCooldown(TipoPocion.Mejora))
        {
            logTimerMejora += Time.deltaTime;
            if (logTimerMejora >= 1f)
            {
                int tiempoRedondeado = Mathf.CeilToInt(GetTiempoRestante(TipoPocion.Mejora));
                Debug.Log("Tiempo restante para Poción de Mejora: " + tiempoRedondeado + "s");
                logTimerMejora = 0f;
            }
        }

        // Llama a la actualización visual en cada frame para asegurar que esté siempre correcto.
        ActualizarEstadoVisual();
    }

    private void ActualizarEstadoVisual()
    {
        if (EstaEnCooldown(TipoPocion.Vida)) BloquearIngredientes(TipoPocion.Vida);
        else DesbloquearIngredientes(TipoPocion.Vida);

        if (EstaEnCooldown(TipoPocion.Mejora)) BloquearIngredientes(TipoPocion.Mejora);
        else DesbloquearIngredientes(TipoPocion.Mejora);
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
            return;
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
            IniciarCooldown(TipoPocion.Vida);
            CrearPocion(pocionVidaPrefab);
            return;
        }

        if (combinacionMejora)
        {
            IniciarCooldown(TipoPocion.Mejora);
            CrearPocion(pocionMejoraPrefab);
            return;
        }

        if (ingredientesEnZona.Count > 1)
        {
            LimpiarMesa();
        }
    }

    void CrearPocion(GameObject prefab)
    {
        // 1. Instanciamos la poción en la mesa (como antes)
        GameObject nuevaPocion = Instantiate(prefab, transform);
        nuevaPocion.transform.SetParent(transform, false);
        if (nuevaPocion.GetComponent<RectTransform>() != null)
        {
            nuevaPocion.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // 2. Determinamos qué prefab de "datos" le corresponde (para el maletín)
        GameObject prefabParaGuardar = null;
        if (prefab == pocionVidaPrefab)
        {
            prefabParaGuardar = prefabPocionVida; // El prefab que guarda los datos, no la UI
        }
        else if (prefab == pocionMejoraPrefab)
        {
            prefabParaGuardar = prefabPocionMejora; // El prefab que guarda los datos, no la UI
        }

        // --- 3. LA PARTE MÁS IMPORTANTE: GUARDAMOS LA POCIÓN EN EL MALETÍN ---
        if (prefabParaGuardar != null && MaletinManager.instancia != null)
        {
            MaletinManager.instancia.GuardarPocion(prefabParaGuardar);
            Debug.Log($"¡Se ha guardado {prefabParaGuardar.name} en el maletín!");
        }

        // 4. Limpiamos los ingredientes de la mesa
        Debug.Log("Poción creada y guardada correctamente.");
        LimpiarMesa();

        //evemto crear pocion
        CustomEvent pocion = new CustomEvent("pocion_creada")
        {
            { "tipo_pocion", prefabParaGuardar.name }
        };
        AnalyticsService.Instance.RecordEvent(pocion);
        AnalyticsService.Instance.Flush();
        //
    }

    void LimpiarMesa()
    {
        foreach (GameObject ingrediente in ingredientesUI)
        {
            if (ingrediente != null)
            {
                // Obtenemos el nombre base (ej: "MejoraB(Clone)" -> "MejoraB")
                string nombreBase = ingrediente.name.Replace("(Clone)", "").Trim();

                // --- LA LÍNEA CLAVE ---
                // Le avisamos al otro script que este ingrediente ya no está "activo"
                // y puede volver a usarse después del cooldown.
                if (IngredienteUIArrastrable.clonesActivos.Contains(nombreBase))
                {
                    IngredienteUIArrastrable.clonesActivos.Remove(nombreBase);
                }

                // Destruimos el objeto visual de la mesa
                Destroy(ingrediente);
            }
        }

        // Limpiamos las listas internas de la mesa
        ingredientesUI.Clear();
        ingredientesEnZona.Clear();
    }

    #region Sistema de Cooldown Persistente

    public enum TipoPocion { Vida, Mejora }

    public void IniciarCooldown(TipoPocion tipo)
    {
        float duracion = tipo == TipoPocion.Vida ? cooldownVida : cooldownMejora;
        DateTime endTime = DateTime.UtcNow.AddSeconds(duracion);

        string key = tipo == TipoPocion.Vida ? CooldownEndTimeVidaKey : CooldownEndTimeMejoraKey;
        PlayerPrefs.SetString(key, endTime.Ticks.ToString());
        PlayerPrefs.Save();

        Debug.Log($"Cooldown iniciado para {tipo}. Termina en {duracion} segundos.");
        BloquearIngredientes(tipo);
    }

    public bool EstaEnCooldown(TipoPocion tipo)
    {
        return GetTiempoRestante(tipo) > 0;
    }

    public float GetTiempoRestante(TipoPocion tipo)
    {
        string key = tipo == TipoPocion.Vida ? CooldownEndTimeVidaKey : CooldownEndTimeMejoraKey;
        string endTimeString = PlayerPrefs.GetString(key, "0");

        if (long.TryParse(endTimeString, out long endTimeTicks))
        {
            DateTime endTime = new DateTime(endTimeTicks);
            TimeSpan restante = endTime - DateTime.UtcNow;
            return restante.TotalSeconds > 0 ? (float)restante.TotalSeconds : 0;
        }
        return 0;
    }
    #endregion

    #region Lógica Visual (Bloqueo/Desbloqueo)

    private void BloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null) return;
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
                if (img != null) img.color = new Color(0.5f, 0.5f, 0.5f, img.color.a);
            }
        }
    }

    // VERSIÓN CORREGIDA
    private void DesbloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null) return;
        foreach (Transform t in panelIngredientes)
        {
            bool esVida = tipo == TipoPocion.Vida && (t.name.Contains("VidaA") || t.name.Contains("VidaB"));
            bool esMejora = tipo == TipoPocion.Mejora && (t.name.Contains("MejoraA") || t.name.Contains("MejoraB"));

            if (esVida || esMejora)
            {
                var cg = t.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    // SE ELIMINÓ LA CONDICIÓN EXTRA. AHORA SIEMPRE FUERZA EL DESBLOQUEO.
                    cg.blocksRaycasts = true;
                    cg.interactable = true; // Asegúrate de tener esta línea también
                    var img = t.GetComponent<Image>();
                    if (img != null) img.color = Color.white;
                }
            }
        }
    }
    #endregion

    void OnDestroy()
    {
        // Si el jugador sale de la escena y queda algún ingrediente en la mesa...
        if (ingredientesUI.Count > 0)
        {
            Debug.Log("Limpiando ingredientes de la mesa al salir de la escena para evitar bloqueos.");

            // Usamos la misma función que ya tienes para limpiar todo correctamente.
            LimpiarMesa();
        }
    }


}