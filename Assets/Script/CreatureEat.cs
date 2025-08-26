using UnityEngine;

public class CreatureEat : MonoBehaviour
{
    public HungerBar hungerBar;         // La barra de hambre de esta criatura
    public CriaturaComidaEsp comidaEsp; // Referencia al script donde definimos la comida aceptada

    public string ComidaAceptada;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ¿Es comida? (tiene tag Food)
        if (collision.CompareTag("Food"))
        {
            comidaEsp = collision.gameObject.GetComponent<CriaturaComidaEsp>();

            // ¿Es la comida que acepta esta criatura?
            if (comidaEsp.comidaAceptada.name==ComidaAceptada)
            {
                hungerBar.Feed(0.2f); // Aumenta la barra
                Destroy(collision.gameObject); // Destruye la comida que fue usada
            }
            else
            {
                Debug.Log($"{gameObject.name} rechazó la comida {collision.name}");
            }
        }
    }
}
