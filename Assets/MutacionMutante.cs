using UnityEngine;

public class MutacionMutante : MonoBehaviour, IMutable
{
    [Header("Sprites de mutación")]
    public Sprite spriteOriginal;
    public Sprite spriteMutado1;
    public Sprite spriteMutadoFinal;

    [Header("Conteo de pociones")]
    public int pocionesNecesariasPrimeraMutacion = 3;
    public int pocionesNecesariasMutacionFinal = 6;

    [Header("Referencia")]
    public float pixelsPorUnidad = 100f;
    public Vector2 tamañoOriginalPixels = new Vector2(1678f, 1356f);

    [Header("Debug / Testing")]
    public bool forzarReinicio = false;
    [SerializeField]
    private int pocionesRecibidas = 0;

    private SpriteRenderer spriteRenderer;
    private static Vector3 escalaOriginalGuardada;
    private static bool escalaInicialDefinida = false;

    private bool yaMutóPrimera = false;
    private bool yaMutóFinal = false;

    private const string PREF_POCIONES = "PocionesMutante";
    private const string PREF_MUTA1 = "MutanteMutado1";
    private const string PREF_MUTAF = "MutanteMutadoFinal";

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!escalaInicialDefinida)
        {
            escalaOriginalGuardada = transform.localScale;
            escalaInicialDefinida = true;
        }

        if (forzarReinicio)
        {
            PlayerPrefs.DeleteKey(PREF_POCIONES);
            PlayerPrefs.DeleteKey(PREF_MUTA1);
            PlayerPrefs.DeleteKey(PREF_MUTAF);
            PlayerPrefs.Save();

            pocionesRecibidas = 0;
            yaMutóPrimera = false;
            yaMutóFinal = false;

            if (spriteRenderer != null && spriteOriginal != null)
            {
                spriteRenderer.sprite = spriteOriginal;
                transform.localScale = escalaOriginalGuardada;
                AjustarEscalaPorTamañoSprite();
                RehacerCollider();
            }

            Debug.Log("🔄 Mutante reiniciado para testing.");
            return;
        }

        if (PlayerPrefs.HasKey(PREF_POCIONES))
        {
            pocionesRecibidas = PlayerPrefs.GetInt(PREF_POCIONES, 0);
            yaMutóPrimera = PlayerPrefs.GetInt(PREF_MUTA1, 0) == 1;
            yaMutóFinal = PlayerPrefs.GetInt(PREF_MUTAF, 0) == 1;

            if (yaMutóFinal)
            {
                AplicarMutacionVisual(spriteMutadoFinal);
            }
            else if (yaMutóPrimera)
            {
                AplicarMutacionVisual(spriteMutado1);
            }
        }
    }

    public void RecibirPocion()
    {
        pocionesRecibidas++;
        GuardarEstado();

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
        GuardarEstado();

        AplicarMutacionVisual(spriteMutado1);
        Debug.Log("🧪 Mutante mutó por primera vez.");
    }

    void MutarFinal()
    {
        yaMutóFinal = true;
        GuardarEstado();

        AplicarMutacionVisual(spriteMutadoFinal);
        Debug.Log("💥 Mutante alcanzó su mutación final.");

        //para final del juego
        GameManager.Instance.NotificarCriaturaMutadaFinal();
    }

    void AplicarMutacionVisual(Sprite nuevoSprite)
    {
        if (spriteRenderer != null && nuevoSprite != null)
        {
            spriteRenderer.sprite = nuevoSprite;
            AjustarEscalaPorTamañoSprite();
            RehacerCollider();
            DesactivarAnimator();
        }
    }

    void AjustarEscalaPorTamañoSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        Vector2 tamañoNuevo = spriteRenderer.sprite.bounds.size;
        Vector2 tamañoOriginalUnidades = tamañoOriginalPixels / pixelsPorUnidad;

        float factorX = tamañoOriginalUnidades.x / Mathf.Max(tamañoNuevo.x, 0.0001f);
        float factorY = tamañoOriginalUnidades.y / Mathf.Max(tamañoNuevo.y, 0.0001f);

        float escalaUniforme = Mathf.Min(factorX, factorY);
        transform.localScale = escalaOriginalGuardada * escalaUniforme;

        Debug.Log($"🔧 Escala ajustada para Mutante: {transform.localScale}");
    }

    void RehacerCollider()
    {
        // CircleCollider2D
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            bool trig = circle.isTrigger;
            PhysicsMaterial2D mat = circle.sharedMaterial;
            Destroy(circle);
            circle = gameObject.AddComponent<CircleCollider2D>();
            circle.isTrigger = trig;
            circle.sharedMaterial = mat;
            Vector2 size = spriteRenderer.sprite.bounds.size;
            circle.radius = Mathf.Max(size.x, size.y) * 0.5f;
        }

        // BoxCollider2D
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            bool trig = box.isTrigger;
            PhysicsMaterial2D mat = box.sharedMaterial;
            Destroy(box);
            box = gameObject.AddComponent<BoxCollider2D>();
            box.isTrigger = trig;
            box.sharedMaterial = mat;
            box.size = spriteRenderer.sprite.bounds.size;
            box.offset = Vector2.zero;
        }

        // PolygonCollider2D
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        if (poly != null)
        {
            bool trig = poly.isTrigger;
            PhysicsMaterial2D mat = poly.sharedMaterial;
            Destroy(poly);
            poly = gameObject.AddComponent<PolygonCollider2D>();
            poly.isTrigger = trig;
            poly.sharedMaterial = mat;
        }
    }

    void DesactivarAnimator()
    {
        Animator animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>() ?? GetComponentInParent<Animator>();
        if (animator != null)
            animator.enabled = false;
    }

    void GuardarEstado()
    {
        PlayerPrefs.SetInt(PREF_POCIONES, pocionesRecibidas);
        PlayerPrefs.SetInt(PREF_MUTA1, yaMutóPrimera ? 1 : 0);
        PlayerPrefs.SetInt(PREF_MUTAF, yaMutóFinal ? 1 : 0);
        PlayerPrefs.Save();
    }
}
