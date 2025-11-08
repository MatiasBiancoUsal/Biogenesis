using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;     // El panel del tutorial
    public GameObject[] imagenes;        // Las 3 imágenes
    private int indiceActual = 0;        // Imagen actual

    void Start()
    {
        // Asegurar que solo la primera imagen esté activa al inicio
        MostrarImagen(0);
        tutorialPanel.SetActive(false);
    }

    public void AbrirOCerrarTutorial()
    {
        bool activo = tutorialPanel.activeSelf;
        tutorialPanel.SetActive(!activo);

        if (!activo)
            MostrarImagen(0);
    }

    public void SiguienteImagen()
    {
        indiceActual++;
        if (indiceActual >= imagenes.Length)
        {
            // Si llegó al final, se cierra el panel
            tutorialPanel.SetActive(false);
            indiceActual = 0;
            return;
        }

        MostrarImagen(indiceActual);
    }

    private void MostrarImagen(int indice)
    {
        for (int i = 0; i < imagenes.Length; i++)
        {
            imagenes[i].SetActive(i == indice);
        }
    }
}
