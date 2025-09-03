using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("Componentes de la UI")]
    public GameObject dialoguePanel;
    public Image dialogueImage;

    [Header("Contenido del Diálogo")]
    public Sprite[] dialogueSprites;

    [Header("Identificador Único de Escena")]
    // IMPORTANTE: Este nombre debe ser único para cada escena con diálogos.
    public string sceneIdentifier;

    private int currentIndex = 0;

    void Start()
    {
        // Verificamos en PlayerPrefs si el diálogo para este identificador ya se vio.
        // GetInt busca la clave. Si no la encuentra, devuelve el valor por defecto (0).
        // Usamos 0 para "no visto" y 1 para "visto".
        if (PlayerPrefs.GetInt(sceneIdentifier, 0) == 1)
        {
            dialoguePanel.SetActive(false); // Si ya se vio (valor es 1), ocultamos el panel y listo.
            return;
        }

        // Si el valor es 0, significa que es la primera vez, así que iniciamos el diálogo.
        StartDialogue();
    }

    void StartDialogue()
    {
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueImage.sprite = dialogueSprites[currentIndex];
    }

    public void NextDialogue()
    {
        currentIndex++;
        if (currentIndex >= dialogueSprites.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueImage.sprite = dialogueSprites[currentIndex];
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        // ¡Paso clave! Guardamos en la memoria que el diálogo de esta escena ya se completó.
        PlayerPrefs.SetInt(sceneIdentifier, 1);
        PlayerPrefs.Save(); // Asegura que el dato se guarde en disco.
    }
}