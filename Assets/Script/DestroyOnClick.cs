using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    void OnMouseDown()
    {
        // Puedes poner efectos, sonidos, puntuaci�n aqu�
        Destroy(gameObject);
    }
}
