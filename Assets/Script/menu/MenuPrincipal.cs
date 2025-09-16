using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    public string gameSceneName = "Juego"; // Cambi� por el nombre real de tu escena

    [Header("UI")]
    public Button continueButton;              // arrastr� el bot�n Continuar
    public GameObject confirmNewGamePanel;     // panel de confirmaci�n (opcional)

    [Header("IDs que us�s en tus criaturas (deben coincidir con creatureID)")]
    public string[] creatureIDs = new string[] { "Alima�a", "Mutante", "Ara�a", "Experimento" };

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
        // Si ten�s panel de confirmaci�n, lo mostr�s.
        if (confirmNewGamePanel != null)
            confirmNewGamePanel.SetActive(true);
        else
            NuevaPartidaConfirmada(); // si no us�s panel, va directo
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
        // Opci�n 1: si us�s una bandera propia "HasSave"
        if (PlayerPrefs.HasKey("HasSave")) return true;

        // Opci�n 2: detectamos si hay alguna key de tus sistemas
        // Vida / Hambre por ID
        foreach (var id in creatureIDs)
        {
            if (PlayerPrefs.HasKey("Vida_" + id)) return true;
            if (PlayerPrefs.HasKey("Hambre_" + id)) return true;
        }

        // Mutaciones (tus claves actuales)
        string[] mutationKeys =
        {
            // Ara�a
            "PocionesAra�a","Ara�aMutada1","Ara�aMutadaFinal",
            // Cazador
            "PocionesCazador","MutoPrimera","MutoFinal",
            // Mutante
            "PocionesMutante","MutanteMutado1","MutanteMutadoFinal",
            // Alima�a
            "PocionesAlima�a","Alima�aMutada1","Alima�aMutadaFinal"
        };
        foreach (var k in mutationKeys)
            if (PlayerPrefs.HasKey(k)) return true;

        return false;
    }
}
