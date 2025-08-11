using UnityEngine;

public class MutacionCazador : MonoBehaviour, IMutable
{
    public Sprite spriteMutado1;
    public Sprite spriteMutadoFinal;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;


    public int pocionesNecesariasPrimeraMutacion = 3;
    public int pocionesNecesariasMutacionFinal = 6;

    private int pocionesRecibidas = 0;
    private bool yaMutóPrimera = false;
    private bool yaMutóFinal = false;

    // ✅ Ponelo acá:
    private Vector2 tamañoOriginalPixels = new Vector2(1872, 1502); // el tamaño del sprite original en píxeles

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        escalaOriginal = transform.localScale;
    }

    public void RecibirPocion()
    {
        pocionesRecibidas++;
        Debug.Log("Cazador recibió poción. Total: " + pocionesRecibidas);

        if (pocionesRecibidas >= pocionesNecesariasMutacionFinal && !yaMutóFinal)
        {
            MutarFinal();
        }
        else if (pocionesRecibidas >= pocionesNecesariasPrimeraMutacion && !yaMutóPrimera)
        {
            MutarPrimera();
        }
    }

    void MutarPrimera()
    {
        yaMutóPrimera = true;

        // Resetear contador para la mutación final
        pocionesRecibidas = 0;

        if (spriteRenderer != null && spriteMutado1 != null)
        {
            spriteRenderer.sprite = spriteMutado1;
            AjustarEscalaPorTamañoSprite();
            Debug.Log("🐛 Mutación 1 activada.");
        }

        // Desactivar animador si está
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;
    }

    void MutarFinal()
    {
        yaMutóFinal = true;

        // Si tenés un sprite final, activalo así:
        if (spriteRenderer != null && spriteMutadoFinal != null)
        {
            spriteRenderer.sprite = spriteMutadoFinal;
            AjustarEscalaPorTamañoSprite(); // También escalá si es otro sprite
            Debug.Log("🧬 Mutación final activada.");
        }

        // Desactivamos Animator también, si hace falta
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;
    }

    void AjustarEscalaPorTamañoSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        // Tamaño en unidades del nuevo sprite
        Vector2 tamañoNuevo = spriteRenderer.sprite.bounds.size;

        // Tamaño del sprite original en unidades (basado en PPU 100)
        Vector2 tamañoOriginal = new Vector2(1872f / 100f, 1502f / 100f);

        float factorX = tamañoOriginal.x / tamañoNuevo.x;
        float factorY = tamañoOriginal.y / tamañoNuevo.y;

        float escalaUniforme = Mathf.Min(factorX, factorY);

        // ✅ Aplicamos el nuevo tamaño relativo a la escala original
        transform.localScale = escalaOriginal * escalaUniforme;

        Debug.Log($"🔧 Escala ajustada: {transform.localScale}");
    }

    // (Opcional) MutarFinal, si querés hacer otra mutación más adelante
}