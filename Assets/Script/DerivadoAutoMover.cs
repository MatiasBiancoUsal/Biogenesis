using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerivadoAutoMover : AutoMover
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // El movimiento base solo se ejecuta si la criatura no está quieta.
        if (!quieto)
        {
            base.Update();
        }

        // Controla la animación de ataque, que se activa cuando 'quieto' es true.
        animator.SetBool("atacar", quieto);
    }
}