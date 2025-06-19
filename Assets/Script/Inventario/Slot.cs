using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegurate de tener esto si us�s TextMesh Pro

[System.Serializable]
public class Slot
{
    public string nombreADN;
    public Image imagenSlot;
    public Sprite spriteActual;
    public int cantidad;
    public TextMeshProUGUI cantidadTexto; // <-- USAR ESTE SI US�S TMP
}

