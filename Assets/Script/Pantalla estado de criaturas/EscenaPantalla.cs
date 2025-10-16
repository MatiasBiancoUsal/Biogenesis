using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EscenaPantalla : MonoBehaviour
{
    public string IrAEscena;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //AnalyticsService.Instance.RecordEvent("ingresarcocina");
        print("evento " + "ingresarcocina ");
        AnalyticsService.Instance.Flush();

        SceneManager.LoadScene(IrAEscena);
    }
}
