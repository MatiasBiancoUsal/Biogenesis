using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 2f;
    public float moveDistance = 3f; // Distancia que se mueve a la izquierda

    private Animator animator;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isWaiting = true;
    private bool goingLeft = true;
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
        targetPos = startPos + Vector3.left * moveDistance;
        timer = waitTime;
        animator.Play("idle");
    }

    void Update()
    {
        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isWaiting = false;
                animator.Play("run");

                // Voltear en la dirección correcta
                FaceDirection(goingLeft ? Vector2.left : Vector2.right);
            }
        }
        else
        {
            Vector3 direction = goingLeft ? Vector3.left : Vector3.right;
            transform.Translate(direction * speed * Time.deltaTime);

            if (goingLeft && transform.position.x <= targetPos.x)
            {
                goingLeft = false;
                isWaiting = true;
                timer = waitTime;
                animator.Play("idle");
            }
            else if (!goingLeft && transform.position.x >= startPos.x)
            {
                goingLeft = true;
                isWaiting = true;
                timer = waitTime;
                animator.Play("idle");
            }
        }
    }

    void FaceDirection(Vector2 dir)
    {
        Vector3 scale = transform.localScale;
        if (dir.x < 0)
            scale.x = Mathf.Abs(scale.x); // Mira a la izquierda (por defecto)
        else
            scale.x = -Mathf.Abs(scale.x); // Mira a la derecha
        transform.localScale = scale;
    }
}
