using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsTool
{
    [MenuItem("Herramientas/Limpiar PlayerPrefs")]
    private static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("¡PlayerPrefs limpiados! Los diálogos se mostrarán de nuevo.");
    }
}