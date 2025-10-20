using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonCargarEscena : MonoBehaviour
{
    public string Controles;

    public void CargarEscena()
    {
        SceneManager.LoadScene(Controles);
    }
}
