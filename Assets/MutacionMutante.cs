using UnityEngine;

public class MutacionMutante : MonoBehaviour, IMutable
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

    private Vector2 tamañoOriginalPixels = new Vector2(1678f, 1356f); // Tamaño original del sprite (px)

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        escalaOriginal = transform.localScale;
    }

    public void RecibirPocion()
    {
        pocionesRecibidas++;
        Debug.Log("☢️ Mutante recibió poción. Total: " + pocionesRecibidas);

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
        pocionesRecibidas = 0;

        if (spriteRenderer != null && spriteMutado1 != null)
        {
            spriteRenderer.sprite = spriteMutado1;
            AjustarEscalaPorTamañoSprite();
            Debug.Log("🧪 Mutante mutó por primera vez.");
        }

        DesactivarAnimator();
    }

    void MutarFinal()
    {
        yaMutóFinal = true;

        if (spriteRenderer != null && spriteMutadoFinal != null)
        {
            spriteRenderer.sprite = spriteMutadoFinal;
            AjustarEscalaPorTamañoSprite();
            Debug.Log("💥 Mutante alcanzó su mutación final.");
        }

        DesactivarAnimator();
    }

    void AjustarEscalaPorTamañoSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        Vector2 tamañoNuevo = spriteRenderer.sprite.bounds.size;
        Vector2 tamañoOriginalUnidades = tamañoOriginalPixels / 100f; // PPU: 100

        float factorX = tamañoOriginalUnidades.x / tamañoNuevo.x;
        float factorY = tamañoOriginalUnidades.y / tamañoNuevo.y;
        float escalaUniforme = Mathf.Min(factorX, factorY);

        transform.localScale = escalaOriginal * escalaUniforme;

        Debug.Log($"🔧 Escala ajustada para Mutante: {transform.localScale}");
    }

    void DesactivarAnimator()
    {
        Animator animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>() ?? GetComponentInParent<Animator>();
        if (animator != null)
            animator.enabled = false;
    }
}
