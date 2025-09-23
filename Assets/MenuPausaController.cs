using UnityEngine;

public class MenuPausaController : MonoBehaviour
{
    public GameObject menuPausa;
    public MonoBehaviour[] scriptsBloqueables; // Ej: EscenaPantalla, Cursormouse

    private bool enPausa = false;

    public void TogglePausa()
    {
        enPausa = !enPausa;

        menuPausa.SetActive(enPausa);

        foreach (MonoBehaviour script in scriptsBloqueables)
        {
            script.gameObject.SetActive(!enPausa);
        }

        Time.timeScale = enPausa ? 0f : 1f;
    }
}
