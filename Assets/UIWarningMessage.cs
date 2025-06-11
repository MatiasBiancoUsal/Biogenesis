using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIWarningMessage : MonoBehaviour
{
    public Text warningText;
    public float messageDuration = 5f;

    public void ShowWarning(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        warningText.text = message;
        warningText.enabled = true;
        yield return new WaitForSeconds(messageDuration);
        warningText.enabled = false;
    }
}
