using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Asegúrate de usar TextMeshPro para la UI

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
            // --- Lógica de UIPersistente.cs ---
            // Esta línea hace que el objeto sea inmortal y sobreviva a los cambios de escena.
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
        // --- Lógica de UIPersistente.cs (Mejorada) ---
        // Esto reemplaza la comprobación que hacías en el Update() de UIPersistente.
        // Se ejecuta solo una vez cuando la escena carga, lo cual es mucho más eficiente.
        ChequearVisibilidadUI(scene.name);
        // =======================================================

        // También actualizamos los textos al cargar una escena nueva.
        ActualizarTextosInventario();
    }

    // =======================================================
    // --- Lógica de ResumenADNUI.cs ---
    // Toda la lógica que tenías en el Update() de ResumenADNUI ahora vive en esta función.
    // Se llama al cargar la escena y cuando recoges un item nuevo para mantener todo sincronizado.
    public void ActualizarTextosInventario()
    {
        // --- CÓDIGO CORREGIDO ---
        Debug.LogWarning(">>> PASO 2: UIGlobalManager recibió el aviso. Actualizando textos ahora.");

        if (InventarioGlobal.Instance == null)
        {
            Debug.LogError("No se puede actualizar la UI porque InventarioGlobal.Instance no existe.");
            return;
        }

        // Usamos los nombres COMPLETOS Y EXACTOS que están en InventarioGlobal.cs
        textoCazador.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Cazador Volador").ToString();
        textoMutante.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Mutante Radiactivo").ToString();
        textoAlimana.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Alimaña Biotecnologica").ToString();
        textoArana.text = InventarioGlobal.Instance.ObtenerCantidad("ADN Araña Mutante").ToString();

        Debug.Log("Textos de la UI Global actualizados con los nuevos valores.");
    }
    // =======================================================

    // =======================================================
    // --- Lógica de AbrirInventarioDesdeResumen.cs ---
    // Esta función reemplaza la que tenías en el otro script.
    // Al estar en un objeto inmortal, el botón del inventario siempre la encontrará.
    public void AbrirPanelInventario()
    {
        Debug.LogWarning(">>> CLIC REGISTRADO: Se llamó a la función AbrirPanelInventario.");

        // Buscamos el panel UNA SOLA VEZ usando su Tag.
        GameObject inventarioPanel = GameObject.FindGameObjectWithTag("InventarioPanel");

        // Comprobamos si se encontró.
        if (inventarioPanel != null)
        {
            Debug.Log("ÉXITO: Se encontró el panel '" + inventarioPanel.name + "' y se está activando.");

            // Lo activamos y lo ponemos al frente de la UI para que se vea sobre todo.
            inventarioPanel.SetActive(true);
            inventarioPanel.transform.SetAsLastSibling();
        }
        else
        {
            // Si no se encontró, mostramos un error claro.
            Debug.LogError(">>> ERROR FATAL: No se encontró NINGÚN objeto con el Tag 'InventarioPanel' en esta escena.");
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