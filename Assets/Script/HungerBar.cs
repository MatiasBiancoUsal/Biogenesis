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

    // Base para cálculo en tiempo real
    private float baseHunger;         // hunger en el momento lastSavedTimestamp
    private long lastSavedTimestamp;  // unix seconds

    // daño por segundo
    private float damageTimer = 0f;

    void Start()
    {
        if (string.IsNullOrWhiteSpace(creatureID))
        {
            creatureID = gameObject.name;
            Debug.LogWarning($"HungerBar: creatureID vacío — uso name '{creatureID}'. Recomiendo asignar un ID único en el Inspector.");
        }

        // Cargar valores guardados
        float savedHunger = PlayerPrefs.GetFloat(GetKey(), 1f);
        string timeStr = PlayerPrefs.GetString(GetTimeKey(), "0");
        long savedTime;
        if (!long.TryParse(timeStr, out savedTime) || savedTime <= 0)
        {
            // si no había timestamp, consideramos que lo guardamos ahora (evita restar un intervalo enorme)
            savedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long elapsed = now - savedTime; // segundos transcurridos

        //calcular hunger al 'now' basado en savedHunger and elapsed
        float computed = Mathf.Clamp01(savedHunger - (float)(elapsed * hungerDecreaseRate));

        // Base para cálculos posteriores (ahora corresponde a 'now')
        baseHunger = computed;
        lastSavedTimestamp = now;
        hungerValue = computed;

        // Marcar que no hay necesidad de guardar inmediatamente
        lastSavedValue = hungerValue;

        UpdateHungerUI();

        if (personajeAsociado == null && !soloMostrar)
            Debug.LogWarning($"HungerBar ({creatureID}): personajeAsociado no asignado.");

        Debug.Log($"HungerBar ({creatureID}) Start: loaded={savedHunger:F3} tSaved={savedTime} elapsed={elapsed}s computed={hungerValue:F3}");
    }
    //

    void Update()
    {
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
                        personajeAsociado.TomarDaño(damagePerSecond);
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
        // si soloMostrar == true: solo mostramos la bajada calculada (no guardamos ni aplicamos daño)
    }

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

    private string GetKey() => "Hambre_" + creatureID.Trim();
    private string GetTimeKey() => "HambreTime_" + creatureID.Trim();
}
