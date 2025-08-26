using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image hungerFill; // Asigna aquí la Image de la barra
    public float hungerValue = 1f; // Entre 0 y 1
    public float hungerDecreaseRate = 0.01f; // Cuánto baja por segundo

    void Update()
    {
        hungerValue -= hungerDecreaseRate * Time.deltaTime;
        hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();
    }

    public void Feed(float amount)
    {
        hungerValue += amount;
        //hungerValue = Mathf.Clamp01(hungerValue);
        UpdateHungerUI();
    }

    void UpdateHungerUI()
    {
        hungerFill.fillAmount = hungerValue/1;
    }
}
