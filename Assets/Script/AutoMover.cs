using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 2f;
    public float moveDistance = 3f; // Distancia que se mueve a la izquierda

    [Header("Audio")]
    public AudioSource audioSource;   // Componente de audio
    public AudioClip walkClip;        // Sonido de caminar/volar

    protected Animator animator;
    protected Vector3 startPos;
    protected Vector3 targetPos;
    protected bool isWaiting = true;
    [SerializeField]public bool goingLeft = true; //MATIAS
    protected float timer;

    [HideInInspector] public bool quieto = false; //  Nueva variable, prueba ataque alimaña
    private Personaje personaje;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
        targetPos = startPos + Vector3.left * moveDistance;
        timer = waitTime;
        animator.Play("idle");
        personaje = GetComponent<Personaje>();
    }

    protected virtual void Update()
    {
        if (personaje != null && personaje.vida <= 0)
        {
            StopWalkingSound(); // Nos aseguramos de que el sonido se detenga.
            return; // No se ejecuta nada más del Update.
        }
        if (quieto) //  Si está quieto, no se mueve
        {
            animator.Play("idle");
            StopWalkingSound();
            return;
        }

        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isWaiting = false;
                animator.Play("run");
                PlayWalkingSound();
                FaceDirection(goingLeft ? Vector2.left : Vector2.right); //MATIAS
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
                StopWalkingSound();
            }
            else if (!goingLeft && transform.position.x >= startPos.x)
            {
                goingLeft = true;
                isWaiting = true;
                timer = waitTime;
                animator.Play("idle");
                StopWalkingSound();
            }
        }
    }

    public virtual void FaceDirection(Vector2 dir)
    {
        Vector3 scale = transform.localScale;
        if (dir.x < 0)
            scale.x = Mathf.Abs(scale.x); // Mira a la izquierda (por defecto)
        else
            scale.x = -Mathf.Abs(scale.x); // Mira a la derecha
        transform.localScale = scale;
    }

    private void PlayWalkingSound()
    {
        if (audioSource != null && walkClip != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkClip;
                audioSource.loop = true; // Repite mientras camina
                audioSource.Play();
            }
        }
    }

    private void StopWalkingSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}