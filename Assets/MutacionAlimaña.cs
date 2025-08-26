using UnityEngine;

public class MutacionAlimaña : MonoBehaviour, IMutable
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
    public Vector2 tamañoOriginalPixels = new Vector2(1642f, 1316f);

    [Header("Debug / Testing")]
    public bool forzarReinicio = false;
    [SerializeField]
    private int pocionesRecibidas = 0;

    private SpriteRenderer spriteRenderer;
    private static Vector3 escalaOriginalGuardada;
    private static bool escalaInicialDefinida = false;

    private bool yaMutóPrimera = false;
    private bool yaMutóFinal = false;

    private const string PREF_POCIONES = "PocionesAlimaña";
    private const string PREF_MUTA1 = "AlimañaMutada1";
    private const string PREF_MUTAF = "AlimañaMutadaFinal";

    //script lucy
    [Header("Daño por mutación")]
    public float multiplicadorDañoPrimera = 1.5f;
    public float multiplicadorDañoFinal = 2.5f;

    public float ObtenerMultiplicadorDaño()
    {
        if (yaMutóFinal)
            return multiplicadorDañoFinal;
        else if (yaMutóPrimera)
            return multiplicadorDañoPrimera;
        else
            return 1f; 
    }
    /////////////
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

            Debug.Log("🔄 Alimaña reiniciada para testing.");
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

        Debug.Log("🧪 Alimaña recibió poción. Total: " + pocionesRecibidas);

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
        Debug.Log("🪱 Alimaña mutó por primera vez.");
    }

    void MutarFinal()
    {
        yaMutóFinal = true;
        GuardarEstado();

        AplicarMutacionVisual(spriteMutadoFinal);
        Debug.Log("🧬 Alimaña alcanzó su mutación final.");
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

        Debug.Log($"🔧 Escala ajustada para Alimaña: {transform.localScale}");
    }

    void RehacerCollider()
    {
        // CapsuleCollider2D
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        if (capsule != null)
        {
            bool trig = capsule.isTrigger;
            PhysicsMaterial2D mat = capsule.sharedMaterial;
            CapsuleDirection2D direction = capsule.direction;

            Destroy(capsule);
            capsule = gameObject.AddComponent<CapsuleCollider2D>();
            capsule.isTrigger = trig;
            capsule.sharedMaterial = mat;
            capsule.direction = direction;

            Vector2 size = spriteRenderer.sprite.bounds.size;
            capsule.size = size;
            capsule.offset = Vector2.zero;
        }

        // También podés agregar soporte para otros tipos de collider si querés.
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