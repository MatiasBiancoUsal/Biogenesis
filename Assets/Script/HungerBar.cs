using System;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image hungerFill;
    [Range(0f, 1f)] public float hungerValue = 1f;
    [Tooltip("Cuánto baja por segundo (ej: 0.01 => 1% por segundo)")]
    public float hungerDecreaseRate = 0.01f;

    [Header("Identificador único de la criatura")]
    public string creatureID = "Alimaña";

    [Header("Opciones")]
    public bool soloMostrar = false; // true en la escena resumen

    [Header("Daño por hambre (solo en escena individual)")]
    public Personaje personajeAsociado;
    public int damagePerSecond = 5;

    [Header("Guardado")]
    public float saveInterval = 1f; // cada cuántos segundos intentamos guardar
    private float saveTimer = 0f;
    private float lastSavedValue = -1f;

    //SOO
    [Header("Control manual")]
    public bool activo = true;

    // Base para cálculo en tiempo real
    private float baseHunger;         // hunger en el momento lastSavedTimestamp
    private long lastSavedTimestamp;  // unix seconds

    // daño por segundo
    private float damageTimer = 0f;

    //SOO
    private bool hasLoaded = false;

    // debug (opcional)
    private float dbgTimer = 0f;

    //SOO también
    void OnEnable()
    {
        LoadSavedState();
        UpdateHungerUI();
        Debug.Log($"HungerBar ({creatureID}) OnEnable: hunger={hungerValue:F3} activo={activo}");
    }

    void Start()
    {
        if (string.IsNullOrWhiteSpace(creatureID))
        {
            creatureID = gameObject.name;
            Debug.LogWarning($"HungerBar: creatureID vacío — uso name '{creatureID}'. Recomiendo asignar un ID único en el Inspector.");
        }

        if (personajeAsociado == null && !soloMostrar)
            Debug.LogWarning($"HungerBar ({creatureID}): personajeAsociado no asignado.");

        // por si por alguna razón OnEnable no se ejecutó antes
        if (!hasLoaded)
            LoadSavedState();
    }
    //

    void Update()
    {
        //SOO
        if (!activo) // si no está activo, solo actualiza la UI pero no baja hambre ni guarda
        {
            UpdateHungerUI();
            return;
        }

        // Calcular hunger actual en base a baseHunger + lastSavedTimestamp (no usamos PlayerPrefs cada frame)
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        double elapsedSinceBase = (double)(now - lastSavedTimestamp);
        hungerValue = Mathf.Clamp01(baseHunger - (float)(elapsedSinceBase * hungerDecreaseRate));
        UpdateHungerUI();

        if (!soloMostrar)
        {
            // daño por hambre en tiempo real (1 segundo de intervalo)
            if (hungerValue <= 0f)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1f)
                {
                    if (personajeAsociado != null)
                        personajeAsociado.TomarDaño(damagePerSecond, "hambre");
                    damageTimer = 0f;
                }
            }
            else damageTimer = 0f;

            // guardado periódico (evita PlayerPrefs cada frame)
            saveTimer += Time.deltaTime;
            if (saveTimer >= saveInterval)
            {
                SaveIfNeeded();
                saveTimer = 0f;
            }
        }

        // debug cada 2s para ver que baja
        dbgTimer += Time.deltaTime;
        if (dbgTimer >= 2f)
        {
            dbgTimer = 0f;
            Debug.Log($"HungerBar ({creatureID}) Update: activo={activo} hunger={hungerValue:F3} base={baseHunger:F3} lastSaved={lastSavedTimestamp}");
        }

        // si soloMostrar == true: solo mostramos la bajada calculada (no guardamos ni aplicamos daño)
    }

    //SOO NUEVO
    private void LoadSavedState()
    {
        float savedHunger = PlayerPrefs.GetFloat(GetKey(), 1f);
        string timeStr = PlayerPrefs.GetString(GetTimeKey(), "0");
        long savedTime;
        if (!long.TryParse(timeStr, out savedTime) || savedTime <= 0)
        {
            savedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsed = now - savedTime;

        float computed = Mathf.Clamp01(savedHunger - (float)(elapsed * hungerDecreaseRate));

        baseHunger = computed;
        lastSavedTimestamp = now;
        hungerValue = computed;

        lastSavedValue = hungerValue;
        hasLoaded = true;

        UpdateHungerUI();
        Debug.Log($"HungerBar ({creatureID}) LoadSavedState: saved={savedHunger:F3} savedTime={savedTime} elapsed={elapsed}s computed={hungerValue:F3}");
    }

    /// <summary>
    /// Activa la pérdida de hambre a partir de ahora.
    /// resetToFull = true -> fuerza hunger a 1.0 al activarse.
    /// </summary>
    public void ActivarHambre(bool resetToFull = false)
    {
        activo = true;
        if (resetToFull) hungerValue = 1f;

        // partir desde el valor actual, con timestamp 'ahora' para que empiece a decrecer.
        baseHunger = hungerValue;
        lastSavedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        UpdateHungerUI();

        Debug.Log($"HungerBar ({creatureID}) ActivarHambre reset={resetToFull} hunger={hungerValue:F3}");
    } //HASTA ACÁ SOO


    public void Feed(float amount)
    {
        // recalculamos current hunger antes de aplicar el feed
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        double elapsedSinceBase = (double)(now - lastSavedTimestamp);
        float current = Mathf.Clamp01(baseHunger - (float)(elapsedSinceBase * hungerDecreaseRate));

        current = Mathf.Clamp01(current + amount);
        hungerValue = current;

        // actualizar base y timestamp (ahora hungerValue corresponde a 'now')
        baseHunger = hungerValue;
        lastSavedTimestamp = now;

        // guardar inmediatamente
        SaveHungerImmediate();

        UpdateHungerUI();
        Debug.Log($"HungerBar ({creatureID}): Feed +{amount} => {hungerValue:F3}");
    }

    void UpdateHungerUI()
    {
        if (hungerFill != null)
            hungerFill.fillAmount = hungerValue;
    }

    private void SaveIfNeeded()
    {
        if (Mathf.Approximately(hungerValue, lastSavedValue)) return;
        SaveHungerImmediate();
    }

    private void SaveHungerImmediate()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        PlayerPrefs.SetFloat(GetKey(), hungerValue);
        PlayerPrefs.SetString(GetTimeKey(), now.ToString()); // guardamos segundos UTC
        PlayerPrefs.Save();

        // actualizamos la base para futuros cálculos (ahora corresponde a 'now')
        baseHunger = hungerValue;
        lastSavedTimestamp = now;
        lastSavedValue = hungerValue;

        Debug.Log($"HungerBar ({creatureID}): saved {hungerValue:F3} at {now}");
    }

    void OnDisable()
    {
        // Guardado final al cambiar de escena / desactivar (solo si no es soloMostrar)
        if (!soloMostrar)
        {
            SaveHungerImmediate();
            Debug.Log($"HungerBar ({creatureID}): OnDisable saved {hungerValue:F3}");
        }
    }

    void OnApplicationQuit()
    {
        if (!soloMostrar)
            SaveHungerImmediate();
    }

    //SOO
    //public void ActivarHambre()
    //{
        //activo = true;

        // recalcular base y timestamp al momento de activarse
        //long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        //baseHunger = hungerValue;
        //lastSavedTimestamp = now;
    //}

    private string GetKey() => "Hambre_" + creatureID.Trim();
    private string GetTimeKey() => "HambreTime_" + creatureID.Trim();
}
