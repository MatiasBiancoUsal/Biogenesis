using UnityEngine;

public class CriaturaReceptor : MonoBehaviour
{
    public InventarioComida inventario;  // arrastr� el mismo asset
    public Sprite comidaFavorita;        // sprite que esta criatura acepta

    public bool IntentarAlimentar()
    {
        if (inventario == null || comidaFavorita == null) return false;

        if (inventario.comidas.Contains(comidaFavorita))
        {
            inventario.RemoverComida(comidaFavorita);
            Debug.Log("�La criatura est� feliz!");
            return true;
        }

        Debug.Log("No es la comida correcta.");
        return false;
    }
}
