using UnityEngine;

public class EstadoCriaturaExperimento : MonoBehaviour
{
    // No necesitamos una referencia manual al manager, lo haremos de forma global.
    // public InventarioManagerPrueba InventarioManagerPrueba; 

    public SpriteRenderer sr;
    public GameObject imgNombre;
    public GameObject barraHP;
    public GameObject barraComida;

    void Awake()
    {
        // Es buena práctica obtener componentes en Awake.
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 1. REVISA EL ESTADO AL INICIAR LA ESCENA.
        // Esto soluciona el caso en que la criatura YA fue creada ANTES de cargar esta escena.
        if (InventarioManagerPrueba.instancia != null && InventarioManagerPrueba.instancia.criaturaCreada)
        {
            ActivarCriatura();
        }
        else
        {
            // Si aún no ha sido creada, nos aseguramos de que todo esté oculto.
            DesactivarCriatura();
        }
    }

    void OnEnable()
    {
        // 2. NOS SUSCRIBIMOS al evento para que nos avisen si se crea MIENTRAS estamos en esta escena.
        InventarioManagerPrueba.OnCriaturaCreada += ActivarCriatura;
    }

    void OnDisable()
    {
        // 3. NOS DESUSCRIBIMOS al salir de la escena para evitar errores.
        InventarioManagerPrueba.OnCriaturaCreada -= ActivarCriatura;
    }

    // Un método que activa todos los componentes visuales.
    void ActivarCriatura()
    {
        Debug.Log("Evento 'OnCriaturaCreada' recibido en la escena de la criatura. Activando sus componentes.");
        if (sr != null) sr.enabled = true;
        if (imgNombre != null) imgNombre.SetActive(true);
        if (barraHP != null) barraHP.SetActive(true);
        if (barraComida != null) barraComida.SetActive(true);
    }

    // Un método que desactiva todos los componentes visuales.
    void DesactivarCriatura()
    {
        if (sr != null) sr.enabled = false;
        if (imgNombre != null) imgNombre.SetActive(false);
        if (barraHP != null) barraHP.SetActive(false);
        if (barraComida != null) barraComida.SetActive(false);
    }
}
