using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    public string gameSceneName = "Juego"; // Cambiá por el nombre real de tu escena

    [Header("UI")]
    public Button continueButton;              // arrastrá el botón Continuar
    public GameObject confirmNewGamePanel;     // panel de confirmación (opcional)

    [Header("IDs que usás en tus criaturas (deben coincidir con creatureID)")]
    public string[] creatureIDs = new string[] { "Alimaña", "Mutante", "Araña", "Experimento" };

    void Awake()
    {
        if (confirmNewGamePanel != null) confirmNewGamePanel.SetActive(false);
    }

    void Start()
    {
        RefreshContinueButton();
    }

    // === BOTONES ===

    public void ContinuarPartida()
    {
        if (!HasSavedGame()) return; // por si queda deshabilitar/seguridad
        // No borramos nada: tus scripts ya cargan de PlayerPrefs.
        SceneManager.LoadScene(gameSceneName);
    }

    public void NuevaPartida()
    {
        // Si tenés panel de confirmación, lo mostrás.
        if (confirmNewGamePanel != null)
            confirmNewGamePanel.SetActive(true);
        else
            NuevaPartidaConfirmada(); // si no usás panel, va directo
    }

    public void NuevaPartidaConfirmada()
    {
        // 1) Borrar TODO lo guardado
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2) Cargar la escena. Tus scripts ya toman valores por defecto:
        //    - Vida: vidaMaxima (por tu BarraVida/Personaje)
        //    - Hambre: 1f (por tu HungerBar)
        //    - Mutaciones: estado inicial (por no existir las keys)
        SceneManager.LoadScene(gameSceneName);
    }

    public void CancelarNuevaPartida()
    {
        if (confirmNewGamePanel != null) confirmNewGamePanel.SetActive(false);
    }

    // === UTILIDADES ===

    private void RefreshContinueButton()
    {
        if (continueButton != null)
            continueButton.interactable = HasSavedGame();
    }

    private bool HasSavedGame()
    {
        // Opción 1: si usás una bandera propia "HasSave"
        if (PlayerPrefs.HasKey("HasSave")) return true;

        // Opción 2: detectamos si hay alguna key de tus sistemas
        // Vida / Hambre por ID
        foreach (var id in creatureIDs)
        {
            if (PlayerPrefs.HasKey("Vida_" + id)) return true;
            if (PlayerPrefs.HasKey("Hambre_" + id)) return true;
        }

        // Mutaciones (tus claves actuales)
        string[] mutationKeys =
        {
            // Araña
            "PocionesAraña","ArañaMutada1","ArañaMutadaFinal",
            // Cazador
            "PocionesCazador","MutoPrimera","MutoFinal",
            // Mutante
            "PocionesMutante","MutanteMutado1","MutanteMutadoFinal",
            // Alimaña
            "PocionesAlimaña","AlimañaMutada1","AlimañaMutadaFinal"
        };
        foreach (var k in mutationKeys)
            if (PlayerPrefs.HasKey(k)) return true;

        return false;
    }
}
