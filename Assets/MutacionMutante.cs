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

    [Header("Efecto visual de mutación")]
    public GameObject prefabHumo;
    public float duracionHumo = 1.5f;

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
            }

            Debug.Log("Mutante reiniciado para testing.");
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

        Debug.Log("Mutante recibió poción. Total: " + pocionesRecibidas);

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
        Debug.Log("Mutante mutó por primera vez.");
    }

    void MutarFinal()
    {
        yaMutóFinal = true;
        GuardarEstado();

        AplicarMutacionVisual(spriteMutadoFinal);
        Debug.Log("Mutante alcanzó su mutación final.");

        // para final del juego
        GameManager.Instance.NotificarCriaturaMutadaFinal();
    }

    void AplicarMutacionVisual(Sprite nuevoSprite)
    {
        if (spriteRenderer == null || nuevoSprite == null)
            return;

        // Instanciar humo en la posición de la criatura
        if (prefabHumo != null)
        {
            GameObject humo = Instantiate(prefabHumo, transform.position, Quaternion.identity);
            Destroy(humo, duracionHumo); // se destruye solo
        }

        // Esperar un poco antes de cambiar sprite
        StartCoroutine(CambiarSpriteConDelay(nuevoSprite));
    }

    private System.Collections.IEnumerator CambiarSpriteConDelay(Sprite nuevoSprite)
    {
        yield return new WaitForSeconds(duracionHumo * 0.8f); // casi al final del humo

        spriteRenderer.sprite = nuevoSprite;
        DesactivarAnimator();
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
