using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    void OnMouseDown()
    {
        // Puedes poner efectos, sonidos, puntuación aquí
        Destroy(gameObject);
    }
}
