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

    private void Start()
    {
        // Cargar hambre guardada
        hungerValue = PlayerPrefs.GetFloat("Hambre_" + creatureID, 1f);
        UpdateHungerUI();
    }
    void Update()
    {
        hungerValue -= hungerDecreaseRate * Time.deltaTime;
        hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();

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
