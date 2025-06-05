using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class Inventario : MonoBehaviour
{
    public GraphicRaycaster graphray;
    private PointerEventData pointerData;
    private List<RaycastResult> rayResult;
    public Transform canvas;
    public GameObject objetosSeleccionado;

    void Start()
    {
        pointerData = new PointerEventData(null);
        rayResult = new List<RaycastResult>();
    }

    void Update()
    {
        Arrastrar();
    }
    void Arrastrar() {
        if (Input.GetMouseButtonDown(0))
        {
            pointerData.position = Input.mousePosition;
            graphray.Raycast(pointerData, rayResult);
            if(rayResult.Count > 0)
            {
                if(rayResult[0].gameObject.GetComponent<Item>()) 
                {
                    objetosSeleccionado = rayResult[0].gameObject;
                    objetosSeleccionado.transform.SetParent(canvas);
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                pointerData.position = Input.mousePosition;
                rayResult.Clear();
                graphray.Raycast(pointerData, rayResult);
                if (rayResult.Count > 0)
                {
                    foreach (var resultado in rayResult) 
                    {
                        if (resultado.gameObject.tag == "Slot")
                        {
                            objetosSeleccionado.transform.SetParent(resultado.gameObject.transform);
                            objetosSeleccionado.transform.localPosition = Vector2.zero;
                        }

                    }
                }
            }
            objetosSeleccionado = null;
        }
        rayResult.Clear ();

    }
}
