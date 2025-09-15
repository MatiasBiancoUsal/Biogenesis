using UnityEngine;

public class MutacionCazador : MonoBehaviour, IMutable
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
    public Vector2 tamañoOriginalPixels = new Vector2(1872, 1502);

    [Header("Debug / Testing")]
    public bool forzarReinicio = false;
    [SerializeField]
    private int pocionesRecibidas = 0;

    private SpriteRenderer spriteRenderer;
    private static Vector3 escalaOriginalGuardada; // 🔹 Se mantiene entre escenas
    private static bool escalaInicialDefinida = false;

    private bool yaMutoPrimera = false;
    private bool yaMutoFinal = false;

    private const string PREF_POCIONES = "PocionesCazador";
    private const string PREF_MUTA1 = "MutoPrimera";
    private const string PREF_MUTAF = "MutoFinal";

    //script lucy
    [Header("Daño por mutación")]
    public float multiplicadorDañoPrimera = 1.5f;
    public float multiplicadorDañoFinal = 2.5f;

    //sofitina
    [Header("Efecto visual de mutación")]
    public AnimatorOverrideController controladorMutacion1;
    public AnimatorOverrideController controladorMutacion2;
    public Animator anim;
    //

    public float ObtenerMultiplicadorDaño()
    {
        if (yaMutoFinal)
            return multiplicadorDañoFinal;
        else if (yaMutoPrimera)
            return multiplicadorDañoPrimera;
        else
            return 1f;
    }

    public EstadoMutacion ObtenerEstadoMutacion()
    {
        if (yaMutoFinal) return EstadoMutacion.Final;
        if (yaMutoPrimera) return EstadoMutacion.Primera;
        return EstadoMutacion.Normal;
    }

    public enum EstadoMutacion
    {
        Normal,
        Primera,
        Final
    }

   
   //////////////
  
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        anim = GetComponent<Animator>();

        // Guardar escala inicial solo una vez
        if (!escalaInicialDefinida)
        {
            escalaOriginalGuardada = transform.localScale;
            escalaInicialDefinida = true;
        }

        // 🔄 Si se activa desde el Inspector, reinicia estado
        if (forzarReinicio)
        {
            PlayerPrefs.DeleteKey(PREF_POCIONES);
            PlayerPrefs.DeleteKey(PREF_MUTA1);
            PlayerPrefs.DeleteKey(PREF_MUTAF);
            PlayerPrefs.Save();

            pocionesRecibidas = 0;
            yaMutoPrimera = false;
            yaMutoFinal = false;

            if (spriteRenderer != null && spriteOriginal != null)
            {
                spriteRenderer.sprite = spriteOriginal;
                transform.localScale = escalaOriginalGuardada;
                RehacerCollider();
            }

            Debug.Log("🔄 Estado reiniciado para pruebas.");
            return;
        }

        // Cargar datos guardados
        if (PlayerPrefs.HasKey(PREF_POCIONES))
        {
            pocionesRecibidas = PlayerPrefs.GetInt(PREF_POCIONES, 0);
            yaMutoPrimera = PlayerPrefs.GetInt(PREF_MUTA1, 0) == 1;
            yaMutoFinal = PlayerPrefs.GetInt(PREF_MUTAF, 0) == 1;

            // Restaurar mutación visualmente
            if (yaMutoFinal)
            {
                AplicarMutacionVisual(spriteMutadoFinal);
            }
            else if (yaMutoPrimera)
            {
                AplicarMutacionVisual(spriteMutado1);
            }
        }
    }

    /// sofitina
    public void Update()
    {
        if (yaMutoPrimera && !yaMutoFinal)
            anim.runtimeAnimatorController = controladorMutacion1;

        else if (yaMutoFinal)
            anim.runtimeAnimatorController = controladorMutacion2;
    }
    //

    public void RecibirPocion()
    {
        pocionesRecibidas++;
        GuardarEstado();

        Debug.Log("Cazador recibió poción. Total: " + pocionesRecibidas);

        if (pocionesRecibidas >= pocionesNecesariasMutacionFinal && !yaMutoFinal)
        {
            MutarFinal();
        }
        else if (pocionesRecibidas >= pocionesNecesariasPrimeraMutacion && !yaMutoPrimera)
        {
            MutarPrimera();
        }
    }

    void MutarPrimera()
    {
        yaMutoPrimera = true;
        pocionesRecibidas = 0; // Resetea para mutación final
        GuardarEstado();

        AplicarMutacionVisual(spriteMutado1);
        Debug.Log("Mutación 1 activada.");
    }

    void MutarFinal()
    {
        yaMutoFinal = true;
        GuardarEstado();

        AplicarMutacionVisual(spriteMutadoFinal);
        Debug.Log("Mutación final activada.");

        //para final del juego
        GameManager.Instance.NotificarCriaturaMutadaFinal();
    }

    void AplicarMutacionVisual(Sprite nuevoSprite)
    {
        if (spriteRenderer != null && nuevoSprite != null)
        {
            spriteRenderer.sprite = nuevoSprite;
            //AjustarEscalaPorTamañoSprite();
            //RehacerCollider();

            //Animator anim = GetComponent<Animator>();
            //if (anim != null) anim.enabled = false;
        }
    }

    void AjustarEscalaPorTamañoSprite()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        Vector2 tamañoOriginalU = tamañoOriginalPixels / pixelsPorUnidad;
        Vector2 tamañoNuevoU = spriteRenderer.sprite.bounds.size;

        float factorX = tamañoOriginalU.x / Mathf.Max(tamañoNuevoU.x, 0.0001f);
        float factorY = tamañoOriginalU.y / Mathf.Max(tamañoNuevoU.y, 0.0001f);
        float factorUniforme = Mathf.Min(factorX, factorY);

        transform.localScale = escalaOriginalGuardada * factorUniforme;
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

    void GuardarEstado()
    {
        PlayerPrefs.SetInt(PREF_POCIONES, pocionesRecibidas);
        PlayerPrefs.SetInt(PREF_MUTA1, yaMutoPrimera ? 1 : 0);
        PlayerPrefs.SetInt(PREF_MUTAF, yaMutoFinal ? 1 : 0);
        PlayerPrefs.Save();
    }
}