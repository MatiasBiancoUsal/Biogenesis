using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsTool
{
    [MenuItem("Herramientas/Limpiar PlayerPrefs")]
    private static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("�PlayerPrefs limpiados! Los di�logos se mostrar�n de nuevo.");
    }
}