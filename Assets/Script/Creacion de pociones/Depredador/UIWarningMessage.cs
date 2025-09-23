using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIWarningMessage : MonoBehaviour
{
    public Text warningText;
    public Image warningIcon;
    public float messageDuration = 5f;

    public void ShowWarning(string message)
    {
        // Detiene cualquier corrutina de alerta previa para evitar superposiciones.
        StopAllCoroutines();
        // Inicia la corrutina para mostrar la nueva alerta.
        StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        // 1. Asegura que el texto y el icono estén activos antes de mostrar el mensaje.
        warningText.enabled = true;
        if (warningIcon != null)
        {
            warningIcon.enabled = true;
        }

        // 2. Asigna el texto que se mostrará en la alerta.
        warningText.text = message;

        // 3. Inicia el efecto de parpadeo para el icono, si existe.
        if (warningIcon != null)
        {
            StartCoroutine(IconFlash());
        }

        // 4. Espera el tiempo de duración especificado para la alerta.
        yield return new WaitForSeconds(messageDuration);

        // 5. Oculta el texto y el icono al finalizar la duración.
        warningText.enabled = false;
        if (warningIcon != null)
        {
            warningIcon.enabled = false;
        }
    }

    IEnumerator IconFlash()
    {
        float flashInterval = 0.5f;
        while (warningIcon.enabled)
        {
            // Cambia el color del icono para crear un efecto de parpadeo suave.
            warningIcon.color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(flashInterval);
            warningIcon.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
