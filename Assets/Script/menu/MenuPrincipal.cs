using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    public string gameSceneName = "Juego";

    [Header("UI")]
    public Button continueButton;
    public GameObject confirmNewGamePanel;

    [Header("IDs que usás en tus criaturas (deben coincidir con creatureID)")]
    public string[] creatureIDs = new string[] { "Alimaña", "Mutante", "Araña", "Experimento" };

    // Constante para la clave de la criatura, para evitar errores de tipeo.
    private const string KEY_CRIATURA_CREADA = "CriaturaCreada";

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
        if (!HasSavedGame()) return;
        SceneManager.LoadScene(gameSceneName);
    }

    //nuevo que agregue TINA
    public void EmpezarPartidaNueva()
    {
        // 1. Borrar TODO lo guardado antes de empezar.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2. Cargar la escena, la cual ahora estará "limpia".
        SceneManager.LoadScene(gameSceneName);
    }
    public void NuevaPartida()
    {
        if (confirmNewGamePanel != null)
            confirmNewGamePanel.SetActive(true);
        else
            NuevaPartidaConfirmada();
    }

    public void NuevaPartidaConfirmada()
    {
        // 1) Borrar TODO lo guardado, incluyendo el estado de la criatura nueva.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2) Cargar la escena.
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
        //  PASO 3: Verificamos si la clave de la criatura existe.
        if (PlayerPrefs.HasKey(KEY_CRIATURA_CREADA)) return true;

        // Opción 1: si usás una bandera propia "HasSave"
        if (PlayerPrefs.HasKey("HasSave")) return true;

        // Opción 2: detectamos si hay alguna key de tus sistemas
        foreach (var id in creatureIDs)
        {
            if (PlayerPrefs.HasKey("Vida_" + id)) return true;
            if (PlayerPrefs.HasKey("Hambre_" + id)) return true;
        }

        string[] mutationKeys =
        {
            "PocionesAraña","ArañaMutada1","ArañaMutadaFinal",
            "PocionesCazador","MutoPrimera","MutoFinal",
            "PocionesMutante","MutanteMutado1","MutanteMutadoFinal",
            "PocionesAlimaña","AlimañaMutada1","AlimañaMutadaFinal"
        };
        foreach (var k in mutationKeys)
            if (PlayerPrefs.HasKey(k)) return true;

        return false;
    }
}