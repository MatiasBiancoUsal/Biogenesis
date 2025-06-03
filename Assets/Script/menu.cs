using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Escena Mapa");
    }

    public void Ajustes()
    {
        SceneManager.LoadScene("Ajustes");
    }
    public void Atras()
    {
        SceneManager.LoadScene("Menu");
    }
}
