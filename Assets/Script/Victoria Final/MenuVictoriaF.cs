using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoriaF : MonoBehaviour
{
    // Reinicia todo el progreso y vuelve al men� inicial
    public void VolverAlMenu()
    {
        // Borra todos los datos guardados (criaturas, inventario, etc.)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Vuelve a la escena del men� principal (cambia el nombre seg�n tu escena)
        SceneManager.LoadScene("Menu");
    }

    // Sale del juego
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
