using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image hungerFill;
    [Range(0f, 1f)] public float hungerValue = 1f;
    public float hungerDecreaseRate = 0.01f;

    [Header("Identificador único de la criatura")]
    public string creatureID = "Alimaña";

    [Header("Opciones")]
    public bool soloMostrar = false; // --> marcar true en la escena resumen

    // codigo sofi
    public Personaje personajeAsociado;
    public int damagePerSecond = 5;
    private float damageTimer = 0f;
    private float damageInterval = 1f;
    // --

    // Guardado optimizado
    public float saveInterval = 1f; // guardar como máximo cada 1s
    private float saveTimer = 0f;
    private float lastSavedValue = -1f;

    void Start()
    {
        hungerValue = PlayerPrefs.GetFloat(GetKey(), 1f);
        hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();

        if (personajeAsociado == null && !soloMostrar)
            Debug.LogWarning($"HungerBar ({creatureID}): personjeAsociado no asignado.");
    }

    void Update()
    {
        if (!soloMostrar)
        {
            // bajar hambre
            hungerValue -= hungerDecreaseRate * Time.deltaTime;
            hungerValue = Mathf.Clamp01(hungerValue);
            UpdateHungerUI();

            // daño por hambre
            if (hungerValue <= 0f)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= damageInterval)
                {
                    if (personajeAsociado != null)
                        personajeAsociado.TomarDaño(damagePerSecond);

                    damageTimer = 0f;
                }
            }
            else damageTimer = 0f;

            // guardado periódico (no cada frame)
            saveTimer += Time.deltaTime;
            if (saveTimer >= saveInterval)
            {
                if (!Mathf.Approximately(hungerValue, lastSavedValue))
                {
                    PlayerPrefs.SetFloat(GetKey(), hungerValue);
                    PlayerPrefs.Save();
                    lastSavedValue = hungerValue;
                }
                saveTimer = 0f;
            }
        }
        else
        {
            // modo solo visual: leer lo guardado y mostrar
            hungerValue = PlayerPrefs.GetFloat(GetKey(), 1f);
            hungerValue = Mathf.Clamp01(hungerValue);
            UpdateHungerUI();
        }
    }

    public void Feed(float amount)
    {
        hungerValue = Mathf.Clamp01(hungerValue + amount);
        UpdateHungerUI();

        PlayerPrefs.SetFloat(GetKey(), hungerValue);
        PlayerPrefs.Save();
        lastSavedValue = hungerValue;
    }

    void UpdateHungerUI()
    {
        if (hungerFill != null)
            hungerFill.fillAmount = hungerValue;
    }

    private string GetKey() => "Hambre_" + creatureID;

    void OnDisable()
    {
        // Guardado final al cambiar de escena / desactivar
        PlayerPrefs.SetFloat(GetKey(), hungerValue);
        PlayerPrefs.Save();
    }
}
