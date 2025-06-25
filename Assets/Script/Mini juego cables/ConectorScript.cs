using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorScript : MonoBehaviour
{
    public enum ColorCable { Rojo, Verde, Azul }
    public ColorCable color;
    public bool isLeft; // ¿Es del lado izquierdo?

    private static ConectorScript seleccionado;

    private void OnMouseDown()
    {
        if (seleccionado == null)
        {
            seleccionado = this;
        }
        else
        {
            if (seleccionado.color == color && seleccionado != this && seleccionado.isLeft != isLeft)
            {
                MiniJuegoManager.Instance.ConectarCables(color, seleccionado, this);
            }
            seleccionado = null;
        }
    }
}
