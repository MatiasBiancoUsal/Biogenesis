using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorCriatura : MonoBehaviour
{
    public GameObject criaturaExperimento;

    void Awake()
    {
        if (criaturaExperimento != null)
            criaturaExperimento.SetActive(false); // Por si quedó activa en escena
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("CriaturaDesbloqueada", 0) == 1)
        {
            criaturaExperimento.SetActive(true);
            Debug.Log("¡Criatura experimento activada!");
        }
    }
}
