using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIWarningMessage : MonoBehaviour
{
    public Text warningText;
    public Image warningIcon; // Nuevo: imagen de advertencia
    public float messageDuration = 5f;

    public void ShowWarning(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        // Activar y mostrar mensaje
        warningText.text = message;
        warningText.enabled = true;

        if (warningIcon != null)
            warningIcon.enabled = true; // Mostrar icono

        // (Opcional) Efecto parpadeo para la imagen
        if (warningIcon != null)
            StartCoroutine(IconFlash());

        yield return new WaitForSeconds(messageDuration);

        // Ocultar después de la duración
        warningText.enabled = false;

        if (warningIcon != null)
            warningIcon.enabled = false;
    }

    IEnumerator IconFlash()
    {
        float flashInterval = 0.5f;
        while (warningIcon.enabled)
        {
            warningIcon.color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(flashInterval);
            warningIcon.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
