using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour
{
    public void escenas (int numeroDeEscena)
      
    {
        SceneManager.LoadScene(numeroDeEscena);
    }
}
