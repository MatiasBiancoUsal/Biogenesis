using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Referencias UI (mismo Canvas)")]
    public GameObject menuPausa;    // Panel overlay
    public GameObject botonPausa;   // Bot�n con el �cono (opcional, para ocultarlo al pausar)

    bool juegoPausado;

    void Awake()
    {
        // Estado inicial consistente
        Time.timeScale = 1f;
        if (menuPausa) menuPausa.SetActive(false);
        if (botonPausa) botonPausa.SetActive(true);
    }

    void Update()
    {
        // Tecla Esc tambi�n alterna pausa
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (juegoPausado) Reanudar();
        else Pausar();
    }

    public void Pausar()
    {
        if (menuPausa) menuPausa.SetActive(true);
        if (botonPausa) botonPausa.SetActive(false); // ocultar el bot�n mientras est� abierto el men�
        Time.timeScale = 0f; // detener el tiempo
        juegoPausado = true;
        // Opcional: AudioListener.pause = true;
    }

    public void Reanudar()
    {
        if (menuPausa) menuPausa.SetActive(false);
        if (botonPausa) botonPausa.SetActive(true);
        Time.timeScale = 1f; // reanudar el tiempo
        juegoPausado = false;
        // Opcional: AudioListener.pause = false;
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f; // por las dudas
        SceneManager.LoadScene("Menu"); // Cambi� por el nombre exacto de tu escena
    }

    public void SalirJuego()
    {
        Application.Quit();
        Debug.Log("Salir del juego"); // visible en el Editor
    }
}
