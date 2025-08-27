using UnityEngine;

public class CreatureEat : MonoBehaviour
{
    public HungerBar hungerBar;         // La barra de hambre de esta criatura
    public CriaturaComidaEsp comidaEsp; // Referencia al script donde definimos la comida aceptada

    //public string ComidaAceptada;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ¿Es comida? (tiene tag Food)
        if (collision.CompareTag("Food"))
        {
            //comidaEsp = collision.gameObject.GetComponent<CriaturaComidaEsp>();

            // Comparamos el prefab que acepta con el objeto que chocó
            if (collision.gameObject.name.Contains(comidaEsp.comidaAceptada.name))
            {
                hungerBar.Feed(0.2f);
                Destroy(collision.gameObject);
                Debug.Log($"{gameObject.name} comió {collision.name}!");
            }
            else
            {
                Debug.Log($"{gameObject.name} rechazó {collision.name}");
            }
        }
    }
}
