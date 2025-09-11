using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image hungerFill; // Asigna aqu� la Image de la barra
    public float hungerValue = 1f; // Entre 0 y 1
    public float hungerDecreaseRate = 0.01f; // Cu�nto baja por segundo
    [Header("Identificador �nico de la criatura")]
    public string creatureID = "Alima�a";
    //  Cambi�s este valor en el Inspector: "Alima�a", "Mutante", "Ara�a" o "Experimento"

    //codigo sofi
    public Personaje personajeAsociado;

    public int damagePerSecond = 5;
    private float damageTimer = 0f;
    private float damageInterval = 1f; // Da�o cada 1 segundo
    //

    private void Start()
    {
        // Cargar hambre guardada
        hungerValue = PlayerPrefs.GetFloat("Hambre_" + creatureID, 1f);
        UpdateHungerUI();

        //codigo sofi
        if (personajeAsociado == null)
        {
            Debug.LogError("No se asign� un personaje al script HungerBar");
        }
        //
    }
    void Update()
    {
        hungerValue -= hungerDecreaseRate * Time.deltaTime;
        hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();

        //codigo sofi
        if (hungerValue <= 0)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                if (personajeAsociado != null)
                {
                    // Llama a la funci�n TomarDa�o del script Personaje
                    personajeAsociado.TomarDa�o(damagePerSecond);
                    Debug.Log("La criatura est� muriendo de hambre. Vida actual: " + personajeAsociado.vida);
                }
                damageTimer = 0f; // Reinicia el temporizador
            }
        }
        else
        {
            damageTimer = 0f; // Reinicia el temporizador si no tiene hambre
        }
        //

        //Guardar autom�ticamente
        PlayerPrefs.SetFloat("Hambre_" + creatureID, hungerValue);
        PlayerPrefs.Save();
    }

    public void Feed(float amount)
    {
        hungerValue += amount;
        //hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();

        PlayerPrefs.SetFloat("Hambre_" + creatureID, hungerValue);
        PlayerPrefs.Save();
    }

    void UpdateHungerUI()
    {
        hungerFill.fillAmount = hungerValue/1;
    }
}
