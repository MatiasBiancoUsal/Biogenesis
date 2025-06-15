using UnityEngine;

public class CreatureEat : MonoBehaviour
{
    public HungerBar hungerBar; // Referencia al script HungerBar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            hungerBar.Feed(0.2f); // Aumenta el alimento
            Destroy(collision.gameObject); // Opcional: destruye la comida
        }
    }
}
