using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FreezeOnAttack : MonoBehaviour
{
    private AutoMover moveScript;             // Script de movimiento
    private CriaturaAttack attackScript;      // Script de ataque
    private Animator animator;                // Controlador de animaciones

    void Start()
    {
        moveScript = GetComponent<AutoMover>();
        attackScript = GetComponent<CriaturaAttack>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (attackScript != null && moveScript != null)
        {
            if (attackScript.currentTarget != null)
            {
                // Hay enemigo: la criatura se queda quieta y ataca
                moveScript.quieto = true;
                animator.ResetTrigger("Idle");    // Resetea el Idle
                animator.SetTrigger("ataque1");   // Dispara animación de ataque
            }
            else
            {
                // No hay enemigo: vuelve a Idle y se puede mover
                moveScript.quieto = false;
                animator.ResetTrigger("ataque1"); // Resetea el ataque
                animator.SetTrigger("Idle");      // Vuelve a Idle
            }
        }
    }
}
