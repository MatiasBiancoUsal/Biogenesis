using UnityEngine;

public class criaturaSpawner : MonoBehaviour
{
    public GameObject criaturaExperimentoPrefab;
    public Transform CriaturaSpawner;

    void Start()
    {
        if (PlayerPrefs.GetInt("CriaturaDesbloqueada", 0) == 1)
        {
            Instantiate(criaturaExperimentoPrefab, CriaturaSpawner.position, CriaturaSpawner.rotation);
            Debug.Log("¡Criatura experimento instanciada en SampleScene!");
        }
    }
}
