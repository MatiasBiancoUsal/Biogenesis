using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPersistente : MonoBehaviour
{
    public static UIPersistente Instance;
    public GameObject UIadn;
    public string[] nombresEscenas;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Update()
    {
       UIadn.SetActive(chequearEscenaActual());
    }



    bool chequearEscenaActual()
    {
        foreach (string x in nombresEscenas)
        {
            Scene escenaActual = SceneManager.GetActiveScene();
            if (escenaActual.name == x) return false;

        }

        return true;
    }
}
