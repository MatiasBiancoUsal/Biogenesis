using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour
{
    public Button muteButton;
    public Image buttonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted;

    private static MuteManager instance;

    void Awake()
    {
        // Singleton para mantener una instancia entre escenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }
    }

    void Start()
    {
        // Cargar estado guardado
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        ApplyMuteState();

        // Escuchar el botón
        muteButton.onClick.AddListener(ToggleMute);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMuteState();
    }

    private void ApplyMuteState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}