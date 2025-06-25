using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajonManager : MonoBehaviour
{
    public GameObject cajonRotoPrefab;
    public Transform spawnPosition;
    public float interval =15f; // 5 minutos

    private GameObject currentCajon;
    private float timer = 0f;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            // Reiniciar estado del minijuego
            PlayerPrefs.SetInt("MiniJuegoResuelto", 0);

            // Instanciar cajón si no existe
            if (currentCajon == null)
            {
                currentCajon = Instantiate(cajonRotoPrefab, spawnPosition.position, Quaternion.identity);
            }
        }

        // Si ya resolviste el minijuego, destruir el cajón si está en escena
        if (PlayerPrefs.GetInt("MiniJuegoResuelto", 0) == 1 && currentCajon != null)
        {
            Destroy(currentCajon);
        }
    }
}
