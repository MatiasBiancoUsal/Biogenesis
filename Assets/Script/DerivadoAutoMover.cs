using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerivadoAutoMover : AutoMover
{

    private Personaje personaje;
    protected override void Start()
    {
        base.Start();
        personaje = GetComponent<Personaje>();
    }

    protected override void Update()
    {
        // --- NUEVO Y MUY IMPORTANTE ---
        // Si la criatura est� incapacitada, detenemos toda la l�gica de este script.
        if (personaje != null && personaje.vida <= 0)
        {
            // Opcional: nos aseguramos de que no se quede en la animaci�n de ataque.
            animator.SetBool("atacar", false);
            return;
        }

        // El movimiento base solo se ejecuta si la criatura no est� quieta.
        if (!quieto)
        {
            base.Update();
        }

        // Controla la animaci�n de ataque, que se activa cuando 'quieto' es true.
        animator.SetBool("atacar", quieto);
    }
}