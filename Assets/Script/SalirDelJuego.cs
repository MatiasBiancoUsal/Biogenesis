using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Esto solo funciona en la build. En el editor no se cerrará,
        // así que mostramos un mensaje para probarlo.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
