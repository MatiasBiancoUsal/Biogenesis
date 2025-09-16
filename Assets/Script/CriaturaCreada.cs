using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaturaCreada : MonoBehaviour
{

    public static CriaturaCreada Instance;

    public bool criaturaCreada = false;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
