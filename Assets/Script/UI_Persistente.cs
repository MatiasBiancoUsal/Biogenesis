using UnityEngine;

public class UIPersistente : MonoBehaviour
{
    public static UIPersistente Instance;
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
}
