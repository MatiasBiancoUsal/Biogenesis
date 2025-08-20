using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ComidaInteractuable : MonoBehaviour
{
    public Sprite comidaSprite;
    public InventarioComida inventario; // arrastrá el mismo asset

    void Reset()
    {
        // Si no tiene sprite, tomarlo del SpriteRenderer del objeto
        if (comidaSprite == null)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) comidaSprite = sr.sprite;
        }
    }

    void OnMouseDown()
    {
        if (inventario == null || comidaSprite == null) return;

        if (inventario.AgregarComida(comidaSprite))
        {
            Debug.Log("Agregada: " + comidaSprite.name);
            // feedback opcional: destello, sonido, etc.
        }
        else
        {
            Debug.Log("Bandeja llena o duplicado no permitido");
        }
    }
}
