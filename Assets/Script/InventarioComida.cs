using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "InventarioComida", menuName = "Juego/Inventario de Comida")]
public class InventarioComida : ScriptableObject
{
    [Tooltip("Comidas actualmente en la bandeja")]
    public List<Sprite> comidas = new List<Sprite>();

    [Min(1)]
    public int maxComidas = 3;

    // Evento para notificar a la UI cuando cambie algo
    public event Action OnCambio;

    [Header("Opcional")]
    public bool permitirDuplicados = true;

    public bool EstaLlena => comidas.Count >= maxComidas;

    public bool AgregarComida(Sprite comida)
    {
        if (EstaLlena) return false;
        if (!permitirDuplicados && comidas.Contains(comida)) return false;

        comidas.Add(comida);
        OnCambio?.Invoke();
        return true;
    }

    public bool RemoverComida(Sprite comida)
    {
        bool ok = comidas.Remove(comida);
        if (ok) OnCambio?.Invoke();
        return ok;
    }

    public void Vaciar()
    {
        if (comidas.Count == 0) return;
        comidas.Clear();
        OnCambio?.Invoke();
    }
}
