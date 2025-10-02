using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIWarningMessage : MonoBehaviour
{
    public Text warningText;
    public Image warningIcon;
    public float messageDuration = 5f;

    Coroutine messageRoutine;
    Coroutine iconFlashRoutine;

    void Awake()
    {
        // Asegurar estado inicial
        if (warningText != null) { warningText.enabled = false; warningText.text = ""; }
        if (warningIcon != null) { warningIcon.enabled = false; warningIcon.color = new Color(1f, 1f, 1f, 1f); }
    }

    public void ShowWarning(string message)
    {
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        if (iconFlashRoutine != null) StopCoroutine(iconFlashRoutine);
        messageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.enabled = true;
        }
        if (warningIcon != null)
        {
            warningIcon.enabled = true;
            iconFlashRoutine = StartCoroutine(IconFlash());
        }

        yield return new WaitForSeconds(messageDuration);

        if (warningText != null) { warningText.enabled = false; warningText.text = ""; }
        if (warningIcon != null) { warningIcon.enabled = false; warningIcon.color = new Color(1f, 1f, 1f, 1f); }

        messageRoutine = null;
    }

    IEnumerator IconFlash()
    {
        float flashInterval = 0.5f;
        while (true)
        {
            if (warningIcon == null) yield break;
            var c = warningIcon.color;
            warningIcon.color = new Color(c.r, c.g, c.b, 0.25f);
            yield return new WaitForSeconds(flashInterval);
            warningIcon.color = new Color(c.r, c.g, c.b, 1f);
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
