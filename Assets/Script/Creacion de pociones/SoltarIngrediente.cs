using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoltarIngrediente : MonoBehaviour, IDropHandler
{
    public List<string> ingredientesEnZona;
    private List<GameObject> ingredientesUI = new List<GameObject>();

    public GameObject pocionVidaPrefab;
    public GameObject pocionMejoraPrefab;

    public GameObject prefabPocionVida;
    public GameObject prefabPocionMejora;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoCreacionPocion;

    [Header("Cooldown")]
    public float cooldownVida = 10f;
    public float cooldownMejora = 10f;
    private static float tiempoRestanteVida = 0f;
    private static float tiempoRestanteMejora = 0f;

    private float contadorLogVida = 0f;
    private float contadorLogMejora = 0f;

    [SerializeField] private Transform panelIngredientes;

    void Awake()
    {
        tiempoRestanteVida = PlayerPrefs.GetFloat("CooldownVida", 0f);
        tiempoRestanteMejora = PlayerPrefs.GetFloat("CooldownMejora", 0f);
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("CooldownVida", tiempoRestanteVida);
        PlayerPrefs.SetFloat("CooldownMejora", tiempoRestanteMejora);
        PlayerPrefs.Save();
    }

    void Start()
    {
        if (tiempoRestanteVida > 0) BloquearIngredientes(TipoPocion.Vida);
        if (tiempoRestanteMejora > 0) BloquearIngredientes(TipoPocion.Mejora);
    }

    void Update()
    {
        if (tiempoRestanteVida > 0)
        {
            tiempoRestanteVida -= Time.deltaTime;
            contadorLogVida += Time.deltaTime;

            if (contadorLogVida >= 1f)
            {
                Debug.Log("Cooldown VIDA: " + Mathf.Ceil(tiempoRestanteVida) + "s");
                contadorLogVida = 0f;
            }

            if (tiempoRestanteVida <= 0) DesbloquearIngredientes(TipoPocion.Vida);
        }

        if (tiempoRestanteMejora > 0)
        {
            tiempoRestanteMejora -= Time.deltaTime;
            contadorLogMejora += Time.deltaTime;

            if (contadorLogMejora >= 1f)
            {
                Debug.Log("Cooldown MEJORA: " + Mathf.Ceil(tiempoRestanteMejora) + "s");
                contadorLogMejora = 0f;
            }

            if (tiempoRestanteMejora <= 0) DesbloquearIngredientes(TipoPocion.Mejora);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Algo fue soltado en la zona");

        GameObject objetoArrastrado = eventData.pointerDrag;

        if (objetoArrastrado != null)
        {
            bool esVida = objetoArrastrado.name.Contains("VidaA") || objetoArrastrado.name.Contains("VidaB");
            bool esMejora = objetoArrastrado.name.Contains("MejoraA") || objetoArrastrado.name.Contains("MejoraB");

            if (esVida && EstaEnCooldown(TipoPocion.Vida)) return;
            if (esMejora && EstaEnCooldown(TipoPocion.Mejora)) return;

            Debug.Log("Se soltó: " + objetoArrastrado.name);

            ingredientesEnZona.Add(objetoArrastrado.name);
            ingredientesUI.Add(objetoArrastrado);

            objetoArrastrado.transform.SetParent(transform, false);

            RevisarCombinaciones();
        }
    }

    void RevisarCombinaciones()
    {
        bool combinacionVida = ingredientesEnZona.Contains("VidaA") && ingredientesEnZona.Contains("VidaB");
        bool combinacionMejora = ingredientesEnZona.Contains("MejoraA") && ingredientesEnZona.Contains("MejoraB");

        if (combinacionVida && !EstaEnCooldown(TipoPocion.Vida))
        {
            Debug.Log("¡Combinación de VIDA detectada!");
            IniciarCooldown(TipoPocion.Vida);
            CrearPocion(pocionVidaPrefab);
            return;
        }

        if (combinacionMejora && !EstaEnCooldown(TipoPocion.Mejora))
        {
            Debug.Log("¡Combinación de MEJORA detectada!");
            IniciarCooldown(TipoPocion.Mejora);
            CrearPocion(pocionMejoraPrefab);
            return;
        }

        if (ingredientesEnZona.Count > 1)
        {
            Debug.Log("Ingredientes no compatibles, eliminándolos.");
            foreach (GameObject ingrediente in ingredientesUI)
            {
                IngredienteUIArrastrable.clonesActivos.Remove(GetBaseName(ingrediente.name));
                Destroy(ingrediente);
            }

            ingredientesUI.Clear();
            ingredientesEnZona.Clear();
        }
    }

    void CrearPocion(GameObject prefab)
    {
        GameObject nuevaPocion = Instantiate(prefab, transform);
        nuevaPocion.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        nuevaPocion.transform.SetParent(transform, false);

        var agarrarPocion = nuevaPocion.GetComponent<AgarrarPocion>();
        if (agarrarPocion != null)
        {
            if (prefab == prefabPocionVida)
                agarrarPocion.prefabParaMaletin = prefabPocionVida;
            else if (prefab == prefabPocionMejora)
                agarrarPocion.prefabParaMaletin = prefabPocionMejora;
        }

        Debug.Log("Poción creada correctamente.");

        // 🔊 Reproducir sonido
        if (audioSource != null && sonidoCreacionPocion != null)
        {
            audioSource.PlayOneShot(sonidoCreacionPocion);
        }

        foreach (GameObject ingrediente in ingredientesUI)
        {
            IngredienteUIArrastrable.clonesActivos.Remove(GetBaseName(ingrediente.name));
            Destroy(ingrediente);
        }

        ingredientesUI.Clear();
        ingredientesEnZona.Clear();
    }

    public enum TipoPocion { Vida, Mejora }

    public bool EstaEnCooldown(TipoPocion tipo)
    {
        return tipo == TipoPocion.Vida ? tiempoRestanteVida > 0 : tiempoRestanteMejora > 0;
    }

    public void IniciarCooldown(TipoPocion tipo)
    {
        if (tipo == TipoPocion.Vida)
        {
            tiempoRestanteVida = cooldownVida;
            BloquearIngredientes(tipo);
        }
        else
        {
            tiempoRestanteMejora = cooldownMejora;
            BloquearIngredientes(tipo);
        }
    }

    private void BloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null)
        {
            Debug.LogError("Panel de ingredientes no asignado.");
            return;
        }

        foreach (Transform t in panelIngredientes)
        {
            bool esVida = tipo == TipoPocion.Vida && (t.name.Contains("VidaA") || t.name.Contains("VidaB"));
            bool esMejora = tipo == TipoPocion.Mejora && (t.name.Contains("MejoraA") || t.name.Contains("MejoraB"));

            if (esVida || esMejora)
            {
                var cg = t.GetComponent<CanvasGroup>() ?? t.gameObject.AddComponent<CanvasGroup>();
                cg.blocksRaycasts = false;
                cg.interactable = false;

                var img = t.GetComponent<UnityEngine.UI.Image>();
                if (img != null) img.color = new Color(0.5f, 0.5f, 0.5f, img.color.a);
            }
        }
    }

    private void DesbloquearIngredientes(TipoPocion tipo)
    {
        if (panelIngredientes == null)
        {
            Debug.LogError("Panel de ingredientes no asignado.");
            return;
        }

        foreach (Transform t in panelIngredientes)
        {
            bool esVida = tipo == TipoPocion.Vida && (t.name.Contains("VidaA") || t.name.Contains("VidaB"));
            bool esMejora = tipo == TipoPocion.Mejora && (t.name.Contains("MejoraA") || t.name.Contains("MejoraB"));

            if (esVida || esMejora)
            {
                var cg = t.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.blocksRaycasts = true;
                    cg.interactable = true;
                }

                var img = t.GetComponent<UnityEngine.UI.Image>();
                if (img != null) img.color = new Color(1f, 1f, 1f, img.color.a);
            }
        }
    }

    string GetBaseName(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }
}