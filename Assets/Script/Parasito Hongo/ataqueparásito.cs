using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ataqueparásito : MonoBehaviour
{
    void Start()
    {
        Scene escenaActual = SceneManager.GetActiveScene();

        if (GlobalParasiteSpawner.instance != null)
        {
            // SOLO si esta es la escena seleccionada para el ataque
            if (GlobalParasiteSpawner.instance.randomScene == escenaActual.name)
            {
                GlobalParasiteSpawner.instance.SpawnearParásito();
            }
        }
    }
}