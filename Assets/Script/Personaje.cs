using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("ID de esta Criatura")]
    [Tooltip("Debe coincidir EXACTAMENTE con el ID usado en el Singleton (ej: 'Araña', 'Alimaña')")]
    public string creatureID;

    [Header("Reflejo de la Vida")]
    [Tooltip("Esta variable es SÓLO un reflejo visual. La vida real se gestiona en el Singleton.")]
    public int vida;
    public int vidaMaxima;

    // (Puedes añadir aquí tus otras variables, como vidaMaxima local si la necesitas)
    // public int vidaMaxima; 


    /// <summary>
    /// Método público para aplicar daño a esta criatura.
    /// </summary>
    /// <param name="daño">La cantidad de daño a recibir (debe ser un número positivo).</param>
    public void RecibirDaño(int daño)
    {
        // Se asegura de que el daño sea positivo antes de enviarlo como negativo
        int cantidadModificar = -Mathf.Abs(daño);

        if (EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.ModificarVida(creatureID, cantidadModificar);
        }
        else
        {
            Debug.LogWarning("Se intentó aplicar daño pero EstadoCriaturasGlobal.instancia es nula.");
        }
    }

    /// <summary>
    /// Método público para curar a esta criatura.
    /// </summary>
    /// <param name="curacion">La cantidad de vida a recuperar (debe ser un número positivo).</param>
    public void Curar(int curacion)
    {
        // Se asegura de que la curación sea positiva
        int cantidadModificar = Mathf.Abs(curacion);

        if (EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.ModificarVida(creatureID, cantidadModificar);
        }
        else
        {
            Debug.LogWarning("Se intentó curar pero EstadoCriaturasGlobal.instancia es nula.");
        }
    }

    /// <summary>
    /// (Opcional) Si quieres que la variable 'vida' de este script 
    /// se actualice visualmente en el Inspector.
    /// </summary>
    void Update()
    {
        if (EstadoCriaturasGlobal.instancia == null) return;

        // Actualiza la variable local 'vida' para que refleje el valor global
        switch (creatureID)
        {
            case "Araña": vida = EstadoCriaturasGlobal.instancia.vidaAraña; break;
            case "Alimaña": vida = EstadoCriaturasGlobal.instancia.vidaAlimaña; break;
            case "Mutante": vida = EstadoCriaturasGlobal.instancia.vidaMutante; break;
            case "Cazador": vida = EstadoCriaturasGlobal.instancia.vidaCazador; break;
        }
    }
}