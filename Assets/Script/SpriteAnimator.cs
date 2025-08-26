using UnityEngine;

[DisallowMultipleComponent]
public class SpriteAnimator : MonoBehaviour
{
    [Header("Renderer objetivo (opcional)")]
    public SpriteRenderer targetRenderer; // si lo dej�s vac�o, lo busca en hijos

    [Header("Velocidad de animaci�n")]
    [Tooltip("Tiempo (seg) entre frames")]
    public float frameRate = 0.1f;

    [Header("Clips actuales")]
    public Sprite[] idleFrames;
    public Sprite[] runFrames;

    [Header("Arranca en Idle")]
    public bool playIdleOnStart = true;

    private SpriteRenderer sr;
    private Sprite[] currentFrames;
    private int frameIndex;
    private float timer;
    private string currentState = "";

    public SpriteRenderer SpriteRenderer => sr; // para que otros scripts puedan usarlo

    void Awake()
    {
        sr = targetRenderer != null ? targetRenderer : GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteAnimator: No encontr� SpriteRenderer. Asign� uno o pon� el renderer como hijo.");
            enabled = false;
        }
    }

    void Start()
    {
        if (playIdleOnStart && idleFrames != null && idleFrames.Length > 0)
            PlayIdle();
    }

    void Update()
    {
        if (currentFrames == null || currentFrames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            frameIndex = (frameIndex + 1) % currentFrames.Length;
            sr.sprite = currentFrames[frameIndex];
        }
    }

    // Cambia los arrays que se usar�n como clips de Idle/Run
    public void SetClips(Sprite[] newIdle, Sprite[] newRun)
    {
        idleFrames = newIdle;
        runFrames = newRun;

        // refresc� si est�s en ese estado
        if (currentState == "Idle" && idleFrames != null && idleFrames.Length > 0)
            PlayIdle();
        else if (currentState == "Run" && runFrames != null && runFrames.Length > 0)
            PlayRun();
    }

    public void PlayIdle()
    {
        if (idleFrames == null || idleFrames.Length == 0) return;
        currentFrames = idleFrames;
        currentState = "Idle";
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentFrames[frameIndex];
    }

    public void PlayRun()
    {
        if (runFrames == null || runFrames.Length == 0) return;
        currentFrames = runFrames;
        currentState = "Run";
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentFrames[frameIndex];
    }

    public void Pause() { enabled = false; }
    public void Resume() { enabled = true; }
}
