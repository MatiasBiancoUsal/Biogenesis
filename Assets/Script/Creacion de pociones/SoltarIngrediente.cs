using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;
    private List<GameObject> ingredientesUI = new List<GameObject>();

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoCreacionPocion;

    [Header("Cooldown")]
    public float cooldownVida = 10f;
    public float cooldownMejora = 10f;

    // Claves para guardar en PlayerPrefs
    private const string CooldownEndTimeVidaKey = "CooldownEndTimeVida";
    private const string CooldownEndTimeMejoraKey = "CooldownEndTimeMejora";

    [SerializeField] private Transform panelIngredientes;

    // Variables para la cuenta atrás en el Log
    private float logTimerVida = 0f;
    private float logTimerMejora = 0f;

    void Start()
    {
        // Al iniciar, revisa los cooldowns y bloquea los ingredientes si es necesario
        ActualizarEstadoIngredientes();
    }

    void Update()
    {
        // Actualiza constantemente el estado de los ingredientes y la cuenta atrás en la consola.
        ActualizarEstadoIngredientes();
    }

    /// <summary>
    /// Revisa si un tipo de poción está en cooldown y actualiza la UI y el log correspondientemente.
    /// </summary>
    private void ActualizarEstadoIngredientes()
    {
        if (EstaEnCooldown(TipoPocion.Vida))
        {
            BloquearIngredientes(TipoPocion.Vida);

            // Lógica para la cuenta atrás en la consola
            logTimerVida += Time.deltaTime;
            if (logTimerVida >= 1f)
            {
                logTimerVida = 0f; // Resetea el cronómetro
                int tiempoRedondeado = Mathf.CeilToInt(GetTiempoRestante(TipoPocion.Vida));
                Debug.Log("Tiempo restante para Poción de Vida: " + tiempoRedondeado + "s");
            }
        }
        else
        {
            DesbloquearIngredientes(TipoPocion.Vida);
        }

        if (EstaEnCooldown(TipoPocion.Mejora))
        {
            BloquearIngredientes(TipoPocion.Mejora);

            // Lógica para la cuenta atrás en la consola
            logTimerMejora += Time.deltaTime;
            if (logTimerMejora >= 1f)
            {
                logTimerMejora = 0f; // Resetea el cronómetro
                int tiempoRedondeado = Mathf.CeilToInt(GetTiempoRestante(TipoPocion.Mejora));
                Debug.Log("Tiempo restante para Poción de Mejora: " + tiempoRedondeado + "s");
            }
        }
        else
        {
            DesbloquearIngredientes(TipoPocion.Mejora);
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
            Debug.Log("Ingrediente en cooldown, no se puede usar.");
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
        Instantiate(prefab, transform.position, Quaternion.identity, transform);

        if (audioSource != null && sonidoCreacionPocion != null)
        {
            audioSource.PlayOneShot(sonidoCreacionPocion);
        }

        LimpiarMesa();
    }

    void LimpiarMesa()
    {
        foreach (GameObject ingrediente in ingredientesUI)
        {
            Destroy(ingrediente);
        }
        ingredientesUI.Clear();
        ingredientesEnZona.Clear();
    }

    public enum TipoPocion { Vida, Mejora }

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

    public void IniciarCooldown(TipoPocion tipo)
    {
        float duracion = tipo == TipoPocion.Vida ? cooldownVida : cooldownMejora;
        DateTime endTime = DateTime.UtcNow.AddSeconds(duracion);

        string key = tipo == TipoPocion.Vida ? CooldownEndTimeVidaKey : CooldownEndTimeMejoraKey;
        PlayerPrefs.SetString(key, endTime.Ticks.ToString());
        PlayerPrefs.Save();

        Debug.Log("Cooldown iniciado para " + tipo + " por " + duracion + " segundos. Termina a las: " + endTime.ToLocalTime());

        BloquearIngredientes(tipo);
    }

    // Función opcional para limpiar los datos guardados desde el Inspector
    [ContextMenu("Limpiar Cooldowns Guardados")]
    public void LimpiarCooldownsGuardados()
    {
        PlayerPrefs.DeleteKey(CooldownEndTimeVidaKey);
        PlayerPrefs.DeleteKey(CooldownEndTimeMejoraKey);
        PlayerPrefs.Save();
        Debug.Log("¡Cooldowns de Vida y Mejora limpiados de PlayerPrefs!");
    }

    #region Lógica de UI (Bloqueo/Desbloqueo)
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

                // --- ESTA ES LA LÍNEA QUE CAMBIAMOS ---
                // Simplemente nos aseguramos de que el CanvasGroup exista.
                if (cg != null)
                {
                    // Al eliminar la condición "!cg.interactable", forzamos
                    // a que siempre se ponga en estado "desbloqueado".
                    cg.blocksRaycasts = true;
                    cg.interactable = true;

                    var img = t.GetComponent<Image>();
                    if (img != null) img.color = new Color(1f, 1f, 1f, img.color.a);
                }
            }
        }
    }
    #endregion
}