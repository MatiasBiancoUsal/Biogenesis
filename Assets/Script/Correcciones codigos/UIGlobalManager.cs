using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Aseg�rate de usar TextMeshPro para la UI

public class UIGlobalManager : MonoBehaviour
{
    public static UIGlobalManager Instance { get; private set; }

    [Header("UI Persistente")]
    public GameObject panelResumenADN;
    public string[] escenasDondeOcultarUI;

    [Header("Textos de Cantidad")]
    public TextMeshProUGUI textoAlimana;
    public TextMeshProUGUI textoArana;
    public TextMeshProUGUI textoCazador;
    public TextMeshProUGUI textoMutante;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            // =======================================================
            // --- L�gica de UIPersistente.cs ---
            // Esta l�nea hace que el objeto sea inmortal y sobreviva a los cambios de escena.
            DontDestroyOnLoad(gameObject);
            // =======================================================
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // =======================================================
        // --- L�gica de UIPersistente.cs (Mejorada) ---
        // Esto reemplaza la comprobaci�n que hac�as en el Update() de UIPersistente.
        // Se ejecuta solo una vez cuando la escena carga, lo cual es mucho m�s eficiente.
        ChequearVisibilidadUI(scene.name);
        // =======================================================

        // Tambi�n actualizamos los textos al cargar una escena nueva.
        ActualizarTextosInventario();
    }

    // =======================================================
    // --- L�gica de ResumenADNUI.cs ---
    // Toda la l�gica que ten�as en el Update() de ResumenADNUI ahora vive en esta funci�n.
    // Se llama al cargar la escena y cuando recoges un item nuevo para mantener todo sincronizado.
    public void ActualizarTextosInventario()
    {
        // --- C�DIGO CORREGIDO ---
        Debug.LogWarning(">>> PASO 2: UIGlobalManager recibi� el aviso. Actualizando textos ahora.");

        if (InventarioGlobal.Instance == null)
        {
            Debug.LogError("No se puede actualizar la UI porque InventarioGlobal.Instance no existe.");
            return;
        }

        // Usamos los nombres COMPLETOS Y EXACTOS que est�n en InventarioGlobal.cs
        textoCazador.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Cazador Volador").ToString();
        textoMutante.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Mutante Radiactivo").ToString();
        textoAlimana.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Alima�a Biotecnologica").ToString();
        textoArana.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Ara�a Mutante").ToString();

        Debug.Log("Textos de la UI Global actualizados con los nuevos valores.");
    }
    // =======================================================

    // =======================================================
    // --- L�gica de AbrirInventarioDesdeResumen.cs ---
    // Esta funci�n reemplaza la que ten�as en el otro script.
    // Al estar en un objeto inmortal, el bot�n del inventario siempre la encontrar�.
    public void AbrirPanelInventario()
    {
        Debug.LogWarning(">>> CLIC REGISTRADO: Se llam� a la funci�n AbrirPanelInventario.");

        // Buscamos el panel UNA SOLA VEZ usando su Tag.
        GameObject inventarioPanel = GameObject.FindGameObjectWithTag("InventarioPanel");

        // Comprobamos si se encontr�.
        if (inventarioPanel != null)
        {
            Debug.Log("�XITO: Se encontr� el panel '" + inventarioPanel.name + "' y se est� activando.");

            // Lo activamos y lo ponemos al frente de la UI para que se vea sobre todo.
            inventarioPanel.SetActive(true);
            inventarioPanel.transform.SetAsLastSibling();
        }
        else
        {
            // Si no se encontr�, mostramos un error claro.
            Debug.LogError(">>> ERROR FATAL: No se encontr� NING�N objeto con el Tag 'InventarioPanel' en esta escena.");
        }
    }

    private void ChequearVisibilidadUI(string nombreEscenaActual)
    {
        bool ocultar = false;
        foreach (string nombre in escenasDondeOcultarUI)
        {
            if (nombreEscenaActual == nombre)
            {
                ocultar = true;
                break;
            }
        }
        panelResumenADN.SetActive(!ocultar);
    }
}