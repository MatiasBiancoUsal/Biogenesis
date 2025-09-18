using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [Header("Componentes de la UI")]
    public GameObject dialoguePanel;
    public Image dialogueImage;

    [Header("Contenido del Diálogo")]
    public Sprite[] dialogueSprites;

    [Header("Sonido del diálogo")]
    public AudioSource dialogueAudio; // arrastrá acá tu AudioSource en el Inspector

    [Header("Identificador Único de Escena")]
    public string sceneIdentifier;

    private int currentIndex = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt(sceneIdentifier, 0) == 1)
        {
            dialoguePanel.SetActive(false);
            return;
        }

        StartDialogue();
    }

    void StartDialogue()
    {
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueImage.sprite = dialogueSprites[currentIndex];

        //  Arrancar sonido
        if (dialogueAudio != null && !dialogueAudio.isPlaying)
        {
            dialogueAudio.loop = true;
            dialogueAudio.Play();
        }
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

        //  Apagar sonido
        if (dialogueAudio != null && dialogueAudio.isPlaying)
        {
            dialogueAudio.Stop();
        }

        PlayerPrefs.SetInt(sceneIdentifier, 1);
        PlayerPrefs.Save();
    }
}