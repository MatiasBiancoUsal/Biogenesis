using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AlarmListener : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Configuración de escena")]
    public string escenaObjetivo = "Cocina"; // solo reproducir en esta escena

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D global
        audioSource.volume = 1f;
    }

    void OnEnable()
    {
        AlarmEvent.OnAlarmTriggered += HandleAlarm;
    }

    void OnDisable()
    {
        AlarmEvent.OnAlarmTriggered -= HandleAlarm;
    }

    private void HandleAlarm(AudioClip clip, float volume)
    {
        // Solo reproducir si estamos en la escena correcta
        if (SceneManager.GetActiveScene().name != escenaObjetivo) return;

        if (clip == null) return;
        audioSource.PlayOneShot(clip, volume);
    }
}
