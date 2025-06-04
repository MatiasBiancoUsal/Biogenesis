using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMouse : MonoBehaviour
{

    public Sprite cursorBlanco;
    public Sprite cursorVerde;
    public LayerMask interactuableLayer;

    SpriteRenderer imagen;

    private void Start()
    {
        imagen = GetComponent<SpriteRenderer>();
        //Cursor.visible = false; //Oculta el cursor del sistema
    }

    // Update is called once per frame
    void Update()
    {
        //mueve el cursor con el mouse
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0;
        transform.position = posicionMouse;

        //Detecta si está sobre un objeto interactuable (con collider y layer marcados)
        Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0f, interactuableLayer);

        if (hit.collider != null)
        {
            imagen.sprite = cursorVerde;
        }
        else
        {
            imagen.sprite = cursorBlanco;
        }

    }
}
