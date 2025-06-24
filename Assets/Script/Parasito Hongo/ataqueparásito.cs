using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ataqueparásito : MonoBehaviour
{
    void Start()
    {
        Scene escenaActual = SceneManager.GetActiveScene();

        if (GlobalParasiteSpawner.instance != null && GlobalParasiteSpawner.instance.sceneNames != null)
        {
            if (GlobalParasiteSpawner.instance.sceneNames.Contains(escenaActual.name))
            {
                GlobalParasiteSpawner.instance.SpawnearParásito();
            }
        }
    }
}